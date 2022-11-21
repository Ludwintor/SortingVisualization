using System.Diagnostics;
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
        private DisplayType _displayType;
        private Bitmap _bitmap;

        public Form1()
        {
            InitializeComponent();
            _generationComboBox.DataSource = Enum.GetValues<GenerationType>();
            _displayComboBox.DataSource = Enum.GetValues<DisplayType>();
            _rng = new Random();
            _brush = new SolidBrush(Color.Gray);
            _pen = new Pen(Color.Gray);
            _pen.Width = 5;
            _currentIndex = -1;
            _bitmap = new Bitmap(_sortPicture.Width, _sortPicture.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _sortPicture.Image = _bitmap;
        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;
            _isBusy = true;
            _currentIndex = -1;
            _currentDelay = (int)_delayCounter.Value;
            _displayType = (DisplayType)_displayComboBox.SelectedItem;
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
            if (_isBusy)
                return;
            SortArray();
        }

        private void SortPicture_Paint(object sender, PaintEventArgs e)
        {
            //if (_array == null)
            //    return;
            //switch ((DisplayType)_displayComboBox.SelectedItem)
            //{
            //    case DisplayType.Columns:
            //        DisplayColumns(e.Graphics);
            //        break;
            //    case DisplayType.SpiralColumns:
            //        DisplaySpiralColumns(e.Graphics);
            //        break;
            //    case DisplayType.Spiral:
            //        break;
            //}
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

        private void DisplaySpiralColumns(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.TranslateTransform(_sortPicture.Width / 2f, _sortPicture.Height / 2f);
            graphics.RotateTransform(180f);
            float angleStep = 360f / _array.Length;
            float width = angleStep * MathF.Log(_array.Length, 12);
            for (int i = 0; i < _array.Length; i++)
            {
                _brush.Color = _currentIndex == i ? Color.Red : Color.Gray;
                float height = Utils.Map(_array[i], 1, _array.Length, width, _sortPicture.Height / 2f);
                graphics.RotateTransform(angleStep);
                graphics.FillRectangle(_brush, 0, 0, width, height);
            }
        }

        private void DisplaySpiral(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.TranslateTransform(_sortPicture.Width / 2f, _sortPicture.Height / 2f);
            float angleStep = 360f / _array.Length;
            PointF prev = PointF.Empty;
            for (int i = 0; i < _array.Length; i++)
            {
                _pen.Color = _currentIndex == i ? Color.Red : Color.Gray;
                float angle = -90f + angleStep * (i + 1);
                float distance = Utils.Map(_array[i], 1, _array.Length, 0, _sortPicture.Height / 2f - _pen.Width);
                float x = distance * MathF.Cos(angle * DEG2RAD);
                float y = distance * MathF.Sin(angle * DEG2RAD);
                PointF current = new(x, y);
                graphics.DrawLine(_pen, prev, current);
                prev = current;
            }
        }

        private async void SortArray()
        {
            _isBusy = true;
            _currentDelay = (int)_delayCounter.Value;
            await Task.Run(() => new IntroSort().Sort(_array, _reverseCheckBox.Checked, null, OnStep)).ConfigureAwait(false);
            _isBusy = false;
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
            using Graphics graphics = Graphics.FromImage(_bitmap);
            switch (_displayType)
            {
                case DisplayType.Columns:
                    DisplayColumns(graphics);
                    break;
                case DisplayType.SpiralColumns:
                    DisplaySpiralColumns(graphics);
                    break;
                case DisplayType.Spiral:
                    DisplaySpiral(graphics);
                    break;
            }
            _sortPicture.Invalidate();
        }
    }
}