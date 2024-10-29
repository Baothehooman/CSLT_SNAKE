using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Snake
{
    /// <summary>
    /// Logic tương tác cho MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window // Định nghĩa lớp MainWindow kế thừa từ Window.
    {
        // Dictionary để ánh xạ các enum GridValue tới các đối tượng ImageSource tương ứng.
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
            {
                { GridValue.Empty, Images.Empty }, // Ánh xạ giá trị lưới Empty tới hình ảnh Empty.
                { GridValue.Snake, Images.Body }, // Ánh xạ giá trị lưới Snake tới hình ảnh Body.
                { GridValue.Food, Images.Food } // Ánh xạ giá trị lưới Food tới hình ảnh Food.
            };

        // Dictionary để ánh xạ các enum Direction tới các góc xoay tương ứng.
        private readonly Dictionary<Direction, int> dirToRotation = new()
            {
                { Direction.Up, 0 }, // Ánh xạ hướng Up tới góc xoay 0 độ.
                { Direction.Right, 90 }, // Ánh xạ hướng Right tới góc xoay 90 độ.
                { Direction.Down, 180 }, // Ánh xạ hướng Down tới góc xoay 180 độ.
                { Direction.Left, 270 } // Ánh xạ hướng Left tới góc xoay 270 độ.
            };

        private readonly int rows = 15, cols = 15; // Định nghĩa số hàng và cột cho lưới.
        private readonly Image[,] gridImages; // Khai báo mảng 2D để chứa các đối tượng Image cho lưới.
        private GameState gameState; // Khai báo biến để chứa trạng thái trò chơi.
        private bool gameRunning; // Khai báo cờ để chỉ ra trò chơi đang chạy.

        public MainWindow() // Constructor cho lớp MainWindow.
        {
            InitializeComponent(); // Khởi tạo các thành phần được định nghĩa trong tệp XAML.
            gridImages = SetupGrid(); // Thiết lập lưới và lưu trữ các đối tượng Image trong gridImages.
            gameState = new GameState(rows, cols); // Khởi tạo trạng thái trò chơi với số hàng và cột đã chỉ định.
        }

        private async Task RunGame() // Phương thức để chạy trò chơi một cách bất đồng bộ.
        {
            Draw(); // Vẽ trạng thái ban đầu của trò chơi.
            await ShowCountDown(); // Hiển thị đếm ngược trước khi bắt đầu trò chơi.
            Overlay.Visibility = Visibility.Hidden; // Ẩn lớp phủ.
            await GameLoop(); // Chạy vòng lặp chính của trò chơi.
            await ShowGameOver(); // Hiển thị màn hình kết thúc trò chơi.
            gameState = new GameState(rows, cols); // Đặt lại trạng thái trò chơi cho một trò chơi mới.
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e) // Trình xử lý sự kiện cho sự kiện nhấn phím xem trước.
        {
            if (Overlay.Visibility == Visibility.Visible) // Nếu lớp phủ đang hiển thị,
            {
                e.Handled = true; // Đánh dấu sự kiện là đã xử lý.
            }

            if (!gameRunning) // Nếu trò chơi đang không chạy,
            {
                gameRunning = true; // Đặt cờ trò chơi đang chạy thành true.
                await RunGame(); // Chạy trò chơi.
                gameRunning = false; // Đặt cờ trò chơi đang chạy thành false sau khi trò chơi kết thúc.
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) // Trình xử lý sự kiện cho sự kiện nhấn phím.
        {
            if (gameState.GameOver) // Nếu trò chơi đã kết thúc,
            {
                return; // Không làm gì cả.
            }

            switch (e.Key) // Chuyển đổi dựa trên phím được nhấn.
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left); // Thay đổi hướng sang trái.
                    break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right); // Thay đổi hướng sang phải.
                    break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up); // Thay đổi hướng lên trên.
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down); // Thay đổi hướng xuống dưới.
                    break;
            }
        }

        private async Task GameLoop() // Phương thức cho vòng lặp chính của trò chơi.
        {
            while (!gameState.GameOver) // Trong khi trò chơi chưa kết thúc,
            {
                await Task.Delay(100); // Chờ 100 mili giây.
                gameState.Move(); // Di chuyển con rắn.
                Draw(); // Vẽ trạng thái cập nhật của trò chơi.
            }
        }

        private Image[,] SetupGrid() // Phương thức để thiết lập lưới.
        {
            Image[,] images = new Image[rows, cols]; // Tạo mảng 2D để chứa các đối tượng Image.
            GameGrid.Rows = rows; // Đặt số hàng trong lưới.
            GameGrid.Columns = cols; // Đặt số cột trong lưới.
            GameGrid.Width = GameGrid.Height * (cols / (double)rows); // Đặt chiều rộng của lưới dựa trên chiều cao và tỷ lệ khung hình.

            for (int r = 0; r < rows; r++) // Lặp qua từng hàng.
            {
                for (int c = 0; c < cols; c++) // Lặp qua từng cột.
                {
                    Image image = new Image // Tạo đối tượng Image mới.
                    {
                        Source = Images.Empty, // Đặt nguồn ban đầu là hình ảnh Empty.
                        RenderTransformOrigin = new Point(0.5, 0.5) // Đặt gốc biến đổi là trung tâm của hình ảnh.
                    };

                    images[r, c] = image; // Lưu trữ đối tượng Image trong mảng 2D.
                    GameGrid.Children.Add(image); // Thêm đối tượng Image vào lưới.
                }
            }

            return images; // Trả về mảng 2D của các đối tượng Image.
        }

        private void Draw() // Phương thức để vẽ trạng thái hiện tại của trò chơi.
        {
            DrawGrid(); // Vẽ lưới.
            DrawSnakeHead(); // Vẽ đầu con rắn.
            ScoreText.Text = $"Score {gameState.Score}"; // Cập nhật văn bản điểm số.
        }

        private void DrawGrid() // Phương thức để vẽ lưới.
        {
            for (int r = 0; r < rows; r++) // Lặp qua từng hàng.
            {
                for (int c = 0; c < cols; c++) // Lặp qua từng cột.
                {
                    GridValue gridVal = gameState.Grid[r, c]; // Lấy giá trị lưới tại vị trí hiện tại.
                    gridImages[r, c].Source = gridValToImage[gridVal]; // Đặt nguồn hình ảnh dựa trên giá trị lưới.
                    gridImages[r, c].RenderTransform = Transform.Identity; // Đặt lại biến đổi.
                }
            }
        }

        private void DrawSnakeHead() // Phương thức để vẽ đầu con rắn.
        {
            Position headPos = gameState.HeadPosition(); // Lấy vị trí của đầu con rắn.
            Image image = gridImages[headPos.Row, headPos.Col]; // Lấy đối tượng Image tại vị trí đầu.
            image.Source = Images.Head; // Đặt nguồn hình ảnh là hình ảnh đầu.

            int rotation = dirToRotation[gameState.Dir]; // Lấy góc xoay dựa trên hướng của con rắn.
            image.RenderTransform = new RotateTransform(rotation); // Áp dụng biến đổi xoay.
        }

        private async Task DrawDeadSnake() // Phương thức để vẽ con rắn đã chết.
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions()); // Lấy các vị trí của con rắn.

            for (int i = 0; i < positions.Count; i++) // Lặp qua từng vị trí.
            {
                Position pos = positions[i]; // Lấy vị trí hiện tại.
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody; // Đặt nguồn hình ảnh dựa trên việc đó là đầu hay thân.
                gridImages[pos.Row, pos.Col].Source = source; // Đặt nguồn hình ảnh.
                await Task.Delay(50); // Chờ 50 mili giây.
            }
        }

        private async Task ShowCountDown() // Phương thức để hiển thị đếm ngược.
        {
            for (int i = 3; i >= 1; i--) // Lặp từ 3 đến 1.
            {
                OverlayText.Text = i.ToString(); // Đặt văn bản lớp phủ là số hiện tại.
                await Task.Delay(500); // Chờ 500 mili giây.
            }
        }

        private async Task ShowGameOver() // Phương thức để hiển thị màn hình kết thúc trò chơi.
        {
            await DrawDeadSnake(); // Vẽ con rắn đã chết.
            await Task.Delay(1000); // Chờ 1 giây.
            Overlay.Visibility = Visibility.Visible; // Hiển thị lớp phủ.
            OverlayText.Text = "PRESS ANY KEY TO START"; // Đặt văn bản lớp phủ để nhắc người dùng bắt đầu trò chơi mới.
        }
    }
}
