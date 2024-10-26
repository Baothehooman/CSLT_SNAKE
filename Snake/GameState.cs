using System; // Nhập không gian tên System để sử dụng các tính năng cơ bản của .NET.
using System.Collections.Generic; // Nhập không gian tên để sử dụng các bộ sưu tập chung như LinkedList.
using System.Linq; // Nhập không gian tên để sử dụng các phương thức mở rộng LINQ.
using System.Text; // Nhập không gian tên để sử dụng các lớp liên quan đến xử lý chuỗi.
using System.Threading.Tasks; // Nhập không gian tên để sử dụng các tác vụ bất đồng bộ.

namespace Snake // Định nghĩa không gian tên cho trò chơi Snake.
{
    public class GameState // Định nghĩa lớp GameState để quản lý trạng thái của trò chơi.
    {
        public int Rows { get; } // Thuộc tính chỉ đọc để lấy số hàng của lưới.
        public int Cols { get; } // Thuộc tính chỉ đọc để lấy số cột của lưới.
        public GridValue[,] Grid { get; } // Thuộc tính chỉ đọc để lấy lưới trò chơi.
        public Direction Dir { get; private set; } // Thuộc tính để lấy và đặt hướng di chuyển của rắn.
        public int Score { get; private set; } // Thuộc tính để lấy và đặt điểm số của trò chơi.
        public bool GameOver { get; private set; } // Thuộc tính để lấy và đặt trạng thái kết thúc trò chơi.

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>(); // Danh sách liên kết để lưu các thay đổi hướng.
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>(); // Danh sách liên kết để lưu các vị trí của rắn.
        private readonly Random random = new Random(); // Đối tượng Random để tạo số ngẫu nhiên.

        public GameState(int rows, int cols) // Constructor để khởi tạo đối tượng GameState.
        {
            Rows = rows; // Gán số hàng.
            Cols = cols; // Gán số cột.
            Grid = new GridValue[rows, cols]; // Khởi tạo lưới trò chơi.
            Dir = Direction.Right; // Đặt hướng di chuyển ban đầu của rắn là hướng phải.

            AddSnake(); // Thêm rắn vào lưới.
            AddFood(); // Thêm thức ăn vào lưới.
        }

        private void AddSnake() // Phương thức để thêm rắn vào lưới.
        {
            int r = Rows / 2; // Tính toán hàng giữa của lưới.

            for (int c = 1; c <= 3; c++) // Vòng lặp để thêm các phần của rắn vào lưới.
            {
                Grid[r, c] = GridValue.Snake; // Đặt giá trị ô lưới là rắn.
                snakePositions.AddFirst(new Position(r, c)); // Thêm vị trí của phần rắn vào danh sách.
            }
        }

        private IEnumerable<Position> EmptyPositions() // Phương thức để lấy các vị trí trống trong lưới.
        {
            for (int r = 0; r < Rows; r++) // Vòng lặp qua các hàng.
            {
                for (int c = 0; c < Cols; c++) // Vòng lặp qua các cột.
                {
                    if (Grid[r, c] == GridValue.Empty) // Kiểm tra nếu ô lưới trống.
                    {
                        yield return new Position(r, c); // Trả về vị trí trống.
                    }
                }
            }
        }

        private void AddFood() // Phương thức để thêm thức ăn vào lưới.
        {
            List<Position> empty = new List<Position>(EmptyPositions()); // Tạo danh sách các vị trí trống.

            if (empty.Count == 0) // Kiểm tra nếu không có vị trí trống.
            {
                return; // Thoát phương thức.
            }

            Position pos = empty[random.Next(empty.Count)]; // Chọn ngẫu nhiên một vị trí trống.
            Grid[pos.Row, pos.Col] = GridValue.Food; // Đặt giá trị ô lưới là thức ăn.
        }

        public Position HeadPosition() // Phương thức để lấy vị trí đầu của rắn.
        {
            return snakePositions.First.Value; // Trả về giá trị đầu tiên trong danh sách vị trí của rắn.
        }

        public Position TailPosition() // Phương thức để lấy vị trí đuôi của rắn.
        {
            return snakePositions.Last.Value; // Trả về giá trị cuối cùng trong danh sách vị trí của rắn.
        }

        public IEnumerable<Position> SnakePositions() // Phương thức để lấy tất cả các vị trí của rắn.
        {
            return snakePositions; // Trả về danh sách vị trí của rắn.
        }

        private void AddHead(Position pos) // Phương thức để thêm đầu mới cho rắn.
        {
            snakePositions.AddFirst(pos); // Thêm vị trí mới vào đầu danh sách vị trí của rắn.
            Grid[pos.Row, pos.Col] = GridValue.Snake; // Đặt giá trị ô lưới là rắn.
        }

        private void RemoveTail() // Phương thức để xóa đuôi của rắn.
        {
            Position tail = snakePositions.Last.Value; // Lấy vị trí đuôi của rắn.
            Grid[tail.Row, tail.Col] = GridValue.Empty; // Đặt giá trị ô lưới là trống.
            snakePositions.RemoveLast(); // Xóa vị trí đuôi khỏi danh sách vị trí của rắn.
        }

        private Direction GetLastDirection() // Phương thức để lấy hướng di chuyển cuối cùng.
        {
            if (dirChanges.Count == 0) // Kiểm tra nếu không có thay đổi hướng.
            {
                return Dir; // Trả về hướng hiện tại.
            }

            return dirChanges.Last.Value; // Trả về giá trị cuối cùng trong danh sách thay đổi hướng.
        }

        private bool CanChangeDirection(Direction newDir) // Phương thức để kiểm tra có thể thay đổi hướng không.
        {
            if (dirChanges.Count == 2) // Kiểm tra nếu đã có 2 thay đổi hướng.
            {
                return false; // Không thể thay đổi hướng.
            }

            Direction lastDir = GetLastDirection(); // Lấy hướng di chuyển cuối cùng.
            return newDir != lastDir && newDir != lastDir.Opposite(); // Kiểm tra nếu hướng mới không trùng với hướng cuối cùng và không phải là hướng ngược lại.
        }

        public void ChangeDirection(Direction dir) // Phương thức để thay đổi hướng di chuyển.
        {
            if (CanChangeDirection(dir)) // Kiểm tra nếu có thể thay đổi hướng.
            {
                dirChanges.AddLast(dir); // Thêm hướng mới vào danh sách thay đổi hướng.
            }
        }

        private bool OutsideGrid(Position pos) // Phương thức để kiểm tra vị trí có nằm ngoài lưới không.
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols; // Trả về true nếu vị trí nằm ngoài lưới.
        }

        private GridValue WillHit(Position newHeadPos) // Phương thức để kiểm tra giá trị ô lưới mà đầu rắn sẽ chạm vào.
        {
            if (OutsideGrid(newHeadPos)) // Kiểm tra nếu vị trí mới nằm ngoài lưới.
            {
                return GridValue.Outside; // Trả về giá trị Outside.
            }

            if (newHeadPos == TailPosition()) // Kiểm tra nếu vị trí mới là vị trí đuôi của rắn.
            {
                return GridValue.Empty; // Trả về giá trị Empty.
            }

            return Grid[newHeadPos.Row, newHeadPos.Col]; // Trả về giá trị ô lưới tại vị trí mới.
        }

        public void Move() // Phương thức để di chuyển rắn.
        {
            if (dirChanges.Count > 0) // Kiểm tra nếu có thay đổi hướng.
            {
                Dir = dirChanges.First.Value; // Lấy hướng mới từ danh sách thay đổi hướng.
                dirChanges.RemoveFirst(); // Xóa hướng mới khỏi danh sách thay đổi hướng.
            }

            Position newHeadPos = HeadPosition().Translate(Dir); // Tính toán vị trí đầu mới của rắn.
            GridValue hit = WillHit(newHeadPos); // Kiểm tra giá trị ô lưới mà đầu rắn sẽ chạm vào.

            if (hit == GridValue.Outside || hit == GridValue.Snake) // Kiểm tra nếu đầu rắn chạm vào ngoài lưới hoặc thân rắn.
            {
                GameOver = true; // Đặt trạng thái kết thúc trò chơi.
            }
            else if (hit == GridValue.Empty) // Kiểm tra nếu đầu rắn chạm vào ô trống.
            {
                RemoveTail(); // Xóa đuôi của rắn.
                AddHead(newHeadPos);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
}
