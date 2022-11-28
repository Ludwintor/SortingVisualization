using System.Drawing.Drawing2D;
using Sorting;

namespace SortingVisualization
{
    public partial class Form1 : Form
    {
        private const float DEG2RAD = MathF.PI / 180f;

        private Random _rng;
        private int[] _array = null!;
        private SolidBrush _brush;
        private Pen _pen;
        private int _currentIndex;
        private bool _isBusy;
        private int _currentDelay;
        private DisplayMode _displayMode;
        private Bitmap _bitmap;

        public Form1()
        {
            InitializeComponent();
            _generationComboBox.DataSource = Enum.GetValues<GenerationType>();
            _displayComboBox.DataSource = Enum.GetValues<DisplayMode>();
            _rng = new Random();
            _brush = new SolidBrush(Color.Gray);
            _pen = new Pen(Color.Gray);
            _pen.Width = 5;
            _currentIndex = -1;
            _bitmap = new Bitmap(_sortPicture.Width, _sortPicture.Height);
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
            _currentIndex = -1;
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
                            _array[i] = _rng.Next(_array.Length);
                            OnStep(i);
                        }
                    }).ConfigureAwait(false);
                    break;
            }
            _sortPicture.Invalidate();
            _isBusy = false;
        }

        private void SortButton_Click(object sender, EventArgs e)
        {
            if (_isBusy || _array == null)
                return;
            SortArray();
        }

        private void SortPicture_Paint(object sender, PaintEventArgs e)
        {
            lock (_bitmap)
            {
                e.Graphics.DrawImage(_bitmap, Point.Empty);
            }
        }

        private void DisplayColumns(Graphics graphics)
        {
            float margin = 1f;
            float maxValue = _sortPicture.Height;
            float width = (float)_sortPicture.Width / _array.Length - margin;
            graphics.Clear(Color.White);
            for (int i = 0; i < _array.Length; i++)
            {
                _brush.Color = _currentIndex == i ? Color.Red : Color.Gray;
                float height = Utils.Map(_array[i], 1, _array.Length, width, maxValue);
                graphics.FillRectangle(_brush, i * (width + margin), maxValue - height, width, height);
            }
        }

        private void DisplayRainbowCircle(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.TranslateTransform(_sortPicture.Width / 2f, _sortPicture.Height / 2f);
            float distance = Math.Min(_sortPicture.Width, _sortPicture.Height) / 2f;
            float angleStep = 360f / _array.Length;
            PointF[] vertices = new PointF[3];
            vertices[0] = PointF.Empty;
            PointF prev = new(0, -distance);
            for (int i = 0; i < _array.Length; i++)
            {
                float angle = -90f + angleStep * (i + 1);
                float colorAngle = Utils.Map(_array[i], 1f, _array.Length, 0f, 360f);
                _brush.Color = _currentIndex == i ? Color.Black : Utils.HSL2RGB(colorAngle, 1d, 0.5d);
                float x = distance * MathF.Cos(angle * DEG2RAD);
                float y = distance * MathF.Sin(angle * DEG2RAD);
                PointF current = new(x, y);
                vertices[1] = prev;
                vertices[2] = current;
                graphics.FillPolygon(_brush, vertices);
                prev = current;
            }
        }

        private void DisplaySpiral(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.TranslateTransform(_sortPicture.Width / 2f, _sortPicture.Height / 2f);
            float angleStep = 360f / _array.Length;
            float distance = Utils.Map(_array[0], 1, _array.Length, 0f, _sortPicture.Height / 2f - _pen.Width);
            PointF prev = new(0, -distance);
            for (int i = 0; i < _array.Length; i++)
            {
                _pen.Color = _currentIndex == i ? Color.Red : Color.Gray;
                float angle = -90f + angleStep * (i + 1);
                distance = Utils.Map(_array[i], 1, _array.Length, angleStep, _sortPicture.Height / 2f - _pen.Width);
                float x = distance * MathF.Cos(angle * DEG2RAD);
                float y = distance * MathF.Sin(angle * DEG2RAD);
                PointF current = new(x, y);
                graphics.DrawLine(_pen, prev, current);
                prev = current;
            }
        }

        private void DisplayRainbowSpiral(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.TranslateTransform(_sortPicture.Width / 2f, _sortPicture.Height / 2f);
            float angleStep = 360f / _array.Length;
            PointF[] vertices = new PointF[3];
            vertices[0] = PointF.Empty;
            float distance = Utils.Map(_array[0], 1, _array.Length, angleStep / 2f, _sortPicture.Height / 2f);
            PointF prev = new(0, -distance);
            for (int i = 0; i < _array.Length; i++)
            {
                float angle = -90f + angleStep * (i + 1);
                distance = Utils.Map(_array[i], 1f, _array.Length, angleStep, _sortPicture.Height / 2f);
                float colorAngle = Utils.Map(_array[i], 1f, _array.Length, 0f, 360f);
                _brush.Color = _currentIndex == i ? Color.Black : Utils.HSL2RGB(colorAngle, 1d, 0.5d);
                float x = distance * MathF.Cos(angle * DEG2RAD);
                float y = distance * MathF.Sin(angle * DEG2RAD);
                PointF current = new(x, y);
                vertices[1] = prev;
                vertices[2] = current;
                graphics.FillPolygon(_brush, vertices);
                prev = current;
            }
        }

        private async void SortArray()
        {
            _isBusy = true;
            _currentDelay = (int)_delayCounter.Value;
            await Task.Run(() => new IntroSort().Sort(_array, _reverseCheckBox.Checked, null, OnStep)).ConfigureAwait(false);
            _isBusy = false;
            _currentIndex = -1;
            Redraw();
            _sortPicture.Invalidate();
        }

        private void OnStep(int index)
        {
            _currentIndex = index;
            Redraw();
            Thread.Sleep(_currentDelay);
        }

        private void Redraw()
        {
            if (_array == null)
                return;
            lock (_bitmap)
            {
                using Graphics graphics = Graphics.FromImage(_bitmap);
                switch (_displayMode)
                {
                    case DisplayMode.Columns:
                        DisplayColumns(graphics);
                        break;
                    case DisplayMode.RainbowFilledCircle:
                        DisplayRainbowCircle(graphics);
                        break;
                    case DisplayMode.Spiral:
                        DisplaySpiral(graphics);
                        break;
                    case DisplayMode.RainbowFilledSpiral:
                        DisplayRainbowSpiral(graphics);
                        break;
                }
            }
            _sortPicture.Invalidate();
        }
    }
}