using Sorting;
using SortingVisualization.Sound;

namespace SortingVisualization
{
    public partial class Form1 : Form
    {
        private const float DEG2RAD = MathF.PI / 180f;

        private readonly Random _rng;
        private readonly SolidBrush _brush;
        private readonly Pen _pen;
        private readonly Bitmap _bitmap;
        private readonly Beeper _beeper;
        private int[] _array = null!;
        private bool _isBusy;
        private int _currentDelay;
        private DisplayMode _displayMode;

        public Form1()
        {
            InitializeComponent();
            _generationComboBox.DataSource = Enum.GetValues<GenerationType>();
            _displayComboBox.DataSource = Enum.GetValues<DisplayMode>();
            _rng = new Random();
            _brush = new SolidBrush(Color.Gray);
            _pen = new Pen(Color.Gray);
            _pen.Width = 5;
            _bitmap = new Bitmap(_sortPicture.Width, _sortPicture.Height);
            _beeper = new Beeper(duration => new SquareWave(duration));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;
            if (_displayComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select display mode", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_generationComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select generation mode", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _isBusy = true;
            _currentDelay = (int)_delayCounter.Value;
            _displayMode = (DisplayMode)_displayComboBox.SelectedItem;
            int count = (int)_elementsCounter.Value;
            switch ((GenerationType)_generationComboBox.SelectedItem)
            {
                case GenerationType.Sequential:
                    _array = Enumerable.Range(1, count).ToArray();
                    await Task.Run(() =>
                    {
                        Utils.BadShuffle(_array, _rng, OnStep);
                    }).ConfigureAwait(false);
                    break;
                case GenerationType.Randomized:
                    _array = new int[count];
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < _array.Length; i++)
                        {
                            _array[i] = _rng.Next(1, _array.Length + 1);
                            OnStep(i);
                        }
                    }).ConfigureAwait(false);
                    break;
            }
            _sortPicture.Invalidate();
            _isBusy = false;
        }

        private async void SortButton_Click(object sender, EventArgs e)
        {
            if (_isBusy || _array == null)
                return;
            await SortArray();
        }

        private void GifButton_Click(object sender, EventArgs e)
        {
            if (_array == null)
                return;
            SaveFileDialog dialog = new()
            {
                Filter = "GIF file|*.gif",
                ValidateNames = true
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            IntroSort sort = new();
            _currentDelay = (int)_delayCounter.Value;
            int[] array = new int[_array.Length];
            _array.CopyTo(array, 0);
            using Bitmap frame = new(_sortPicture.Width, _sortPicture.Height);
            using GifWriter writer = new(dialog.FileName);
            Redraw(array, frame, -1);
            writer.WriteFrame(frame);
            sort.Sort(array, _reverseCheckBox.Checked, null, i => GifStep(array, frame, writer, i));
            Redraw(array, frame, -1);
            writer.WriteFrame(frame, 2000);
            MessageBox.Show("GIF created!");
        }

        private void SortPicture_Paint(object sender, PaintEventArgs e)
        {
            lock (_bitmap)
            {
                e.Graphics.DrawImage(_bitmap, Point.Empty);
            }
        }

        private void DisplayColumns(int[] array, Graphics graphics, int index)
        {
            float margin = 1f;
            float maxValue = graphics.VisibleClipBounds.Height;
            float width = graphics.VisibleClipBounds.Width / array.Length - margin;
            graphics.Clear(Color.White);
            for (int i = 0; i < array.Length; i++)
            {
                _brush.Color = index == i ? Color.Red : Color.Gray;
                float height = Utils.Map(array[i], 1, array.Length, width, maxValue);
                graphics.FillRectangle(_brush, i * (width + margin), maxValue - height, width, height);
            }
        }

        private void DisplayRainbowCircle(int[] array, Graphics graphics, int index)
        {
            graphics.Clear(Color.White);
            RectangleF rect = graphics.VisibleClipBounds;
            graphics.TranslateTransform(rect.Width / 2f, rect.Height / 2f);
            float distance = Math.Min(rect.Width, rect.Height) / 2f;
            float angleStep = 360f / array.Length;
            PointF[] vertices = new PointF[3];
            vertices[0] = PointF.Empty;
            PointF prev = new(0, -distance);
            for (int i = 0; i < array.Length; i++)
            {
                float angle = -90f + angleStep * (i + 1);
                float colorAngle = Utils.Map(array[i], 1f, array.Length, 0f, 360f);
                _brush.Color = index == i ? Color.Black : Utils.HSL2RGB(colorAngle, 1d, 0.5d);
                float x = distance * MathF.Cos(angle * DEG2RAD);
                float y = distance * MathF.Sin(angle * DEG2RAD);
                PointF current = new(x, y);
                vertices[1] = prev;
                vertices[2] = current;
                graphics.FillPolygon(_brush, vertices);
                prev = current;
            }
        }

        private void DisplaySpiral(int[] array, Graphics graphics, int index)
        {
            graphics.Clear(Color.White);
            RectangleF rect = graphics.VisibleClipBounds;
            graphics.TranslateTransform(rect.Width / 2f, rect.Height / 2f);
            float angleStep = 360f / array.Length;
            float distance = Utils.Map(array[0], 1, array.Length, 0f, rect.Height / 2f - _pen.Width);
            PointF prev = new(0, -distance);
            for (int i = 0; i < array.Length; i++)
            {
                _pen.Color = index == i ? Color.Red : Color.Gray;
                float angle = -90f + angleStep * (i + 1);
                distance = Utils.Map(_array[i], 1, _array.Length, angleStep, rect.Height / 2f - _pen.Width);
                float x = distance * MathF.Cos(angle * DEG2RAD);
                float y = distance * MathF.Sin(angle * DEG2RAD);
                PointF current = new(x, y);
                graphics.DrawLine(_pen, prev, current);
                prev = current;
            }
        }

        private void DisplayRainbowSpiral(int[] array, Graphics graphics, int index)
        {
            graphics.Clear(Color.White);
            RectangleF rect = graphics.VisibleClipBounds;
            graphics.TranslateTransform(rect.Width / 2f, rect.Height / 2f);
            float angleStep = 360f / array.Length;
            PointF[] vertices = new PointF[3];
            vertices[0] = PointF.Empty;
            float distance = Utils.Map(array[0], 1, array.Length, angleStep / 2f, rect.Height / 2f);
            PointF prev = new(0, -distance);
            for (int i = 0; i < array.Length; i++)
            {
                float angle = -90f + angleStep * (i + 1);
                distance = Utils.Map(array[i], 1f, array.Length, angleStep, rect.Height / 2f);
                float colorAngle = Utils.Map(array[i], 1f, array.Length, 0f, 360f);
                _brush.Color = index == i ? Color.Black : Utils.HSL2RGB(colorAngle, 1d, 0.5d);
                float x = distance * MathF.Cos(angle * DEG2RAD);
                float y = distance * MathF.Sin(angle * DEG2RAD);
                PointF current = new(x, y);
                vertices[1] = prev;
                vertices[2] = current;
                graphics.FillPolygon(_brush, vertices);
                prev = current;
            }
        }

        private async Task SortArray()
        {
            _isBusy = true;
            _currentDelay = (int)_delayCounter.Value;
            await Task.Run(() => new IntroSort().Sort(_array, _reverseCheckBox.Checked, null, OnStep)).ConfigureAwait(false);
            _beeper.Stop();
            _isBusy = false;
            Redraw(_array, _bitmap, -1);
            _sortPicture.Invalidate();
        }

        private void OnStep(int index)
        {
            Redraw(_array, _bitmap, index);
            _sortPicture.Invalidate();
            int freq = (int)Utils.Map(_array[index], 1, _array.Length, 100, 1000);
            _beeper.Beep(freq, 0.05f, Math.Max(_currentDelay, 80));
            Thread.Sleep(_currentDelay);
        }

        private void GifStep(int[] array, Bitmap frame, GifWriter writer, int index)
        {
            Redraw(array, frame, index);
            writer.WriteFrame(frame);
        }

        private void Redraw(int[] array, Bitmap bitmap, int index)
        {
            if (array == null)
                return;
            lock (bitmap)
            {
                using Graphics graphics = Graphics.FromImage(bitmap);
                switch (_displayMode)
                {
                    case DisplayMode.Columns:
                        DisplayColumns(array, graphics, index);
                        break;
                    case DisplayMode.RainbowFilledCircle:
                        DisplayRainbowCircle(array, graphics, index);
                        break;
                    case DisplayMode.Spiral:
                        DisplaySpiral(array, graphics, index);
                        break;
                    case DisplayMode.RainbowFilledSpiral:
                        DisplayRainbowSpiral(array, graphics, index);
                        break;
                }
            }
        }
    }
}