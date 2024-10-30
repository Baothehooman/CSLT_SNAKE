using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class GameState
    {
        public int Rows { get; } //số cột và dòng trong lưới ô
        public int Cols { get; }
        public GridValue[,] Grid { get; } //mảng 2 chiều tạo lưới ô (hàm get theo em hiểu là chỉ có thể đọc)
        public Direction Dir { get; private set; } //hướng di chuyển của con rắn; get-set là cổng truy cập dữ liệu (get cho phép truy cập và đọc, set cho phép sửa đổi dữ liệu)
        public int Score { get; private set; } 
        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>(); //Danh sách liên kết lưu các thay đổi hướng di chuyển
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>(); //Danh sách liên kết lưu các vị trí của rắn (yếu tố đầu tiên là đầu rắn, cuối cùng là đuôi rắn)
        private readonly Random random = new Random(); //random vị trí chứa mồi

        public GameState(int rows, int cols) //Khởi tạo trò chơi với số hàng và cột cụ thể
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols]; //Thiết lập lưới bằng mảng 2 chiều
            Dir = Direction.Right; //khi bắt đầu game, hướng di chuyển ban đầu của con rắn là bên phải

            AddSnake(); //thêm rắn và thực phẩm vào lưới
            AddFood();
        }

        private void AddSnake() //thêm rắn vào lưới ô bắt đầu chơi
        {
            int r = Rows / 2; //khai báo biến đặt rắn tại vị trí giữa số hàng tính từ trên xuống

            for (int c = 1; c <= 3; c++) //lặp 3 cột chứa rắn (đầu thân đuôi)
            {
                Grid[r, c] = GridValue.Snake; //giá trị ô chứa rắn trong lưới ô ??
                snakePositions.AddFirst(new Position(r, c)); //thêm vào các vị trí của rắn trong danh sách vị trí
            }
        }

        private IEnumerable<Position> EmptyPositions() //tìm tất cả các vị trí trống trong lưới và trả về thông qua yield return;
                                                       //IEnumerable là 1 danh sách chỉ đọc và chỉ duyệt 1 chiều bằng foreach;
                                                       //Yield là từ khóa sẽ báo hiệu cho trình biên dịch biết rằng phương thức, toán tử, get mà nó xuất hiện sẽ là một khối lặp. Trình biên dịch sẽ tự động sinh ra một class implement từ IEnumerable, IEnumerator để thể hiện khối lặp đó.
                                                       //Nếu là hàm sử dụng yield return thì chương trình sẽ trả về dữ liệu và quay lại vòng lặp để thực hiện vòng lặp tiếp theo.
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Position(r, c); //nếu ô đang đc duyệt trống thì trả về gtri ô đó và quay lại vòng lặp ktra các ô tiếp theo
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions()); //khởi tạo một danh sách "empty" chứa các vị trí trống

            if (empty.Count == 0) //nếu không còn ô nào trống
            {
                return; //Nếu danh sách rỗng (empty.Count == 0), phương thức kết thúc và không thực hiện thêm thao tác nào.
                        //Điều này ngăn chặn việc cố gắng đặt mồi vào lưới khi không còn vị trí trống.
            }

            Position pos = empty[random.Next(empty.Count)]; //chọn random một vị trí trống, trả về một số ngẫu nhiên trong khoảng từ 0 đến empty.Count - 1, và vị trí tương ứng được gán cho biến pos
            Grid[pos.Row, pos.Col] = GridValue.Food; //đặt mồi vào trong vị trí trống đó
        }

        public Position HeadPosition() //phương thức trả về vị trí của đầu rắn
        {
            return snakePositions.First.Value; //snakePositions.First.Value truy cập vào phần tử đầu tiên trong danh sách liên kết snakePositions, đó là vị trí của đầu rắn
        }

        public Position TailPosition() //Phương thức trả về vị trí của đuôi rắn
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions() //Phương thức trả về tất cả các vị trí của rắn dưới dạng một chuỗi các phần tử
        {
            return snakePositions;
        }

        private void AddHead(Position pos) //Phương thức thêm một vị trí mới vào đầu rắn (pos)
        {
            snakePositions.AddFirst(pos); //vị trí mới đc gán cho biến pos, pos đc thêm vào đầu danh sách snakePosition
            Grid[pos.Row, pos.Col] = GridValue.Snake; //vị trí pos đc cập nhật trong lưới ô
        }

        private void RemoveTail() //xóa vị trí cuối cùng của đuôi rắn
        {
            Position tail = snakePositions.Last.Value; //biến chứa vị trí đuôi rắn
            Grid[tail.Row, tail.Col] = GridValue.Empty; // cập nhật lưới ô tạo vị trí đuôi rắn với gtri empty
            snakePositions.RemoveLast(); //xóa vị trí đó khỏi danh sách snakePosition
        }

        private Direction GetLastDirection() //trả về hướng di chuyển cuối cùng của rắn
        {
            if (dirChanges.Count == 0) //Nếu danh sách dirChanges trống, nó trả về hướng hiện tại Dir
                                       
            {
                return Dir;
            }

            return dirChanges.Last.Value; //ngược lại nó trả về giá trị cuối cùng trong danh sách dirChanges
        }

        private bool CanChangeDirection(Direction newDir) //Kiểm tra xem có thể thay đổi hướng di chuyển của rắn hay không
        {
            if (dirChanges.Count == 2) //Kiểm tra xem số lượng thay đổi hướng đã đạt đến giới hạn là 2 hay chưa
            {
                return false; //Nếu số lượng thay đổi hướng là 2, trả về false (không thể thay đổi thêm hướng)
            }

            Direction lastDir = GetLastDirection(); //Lấy hướng di chuyển cuối cùng từ danh sách các thay đổi hướng
            return newDir != lastDir && newDir != lastDir.Opposite(); //trả về hướng mới; ktra hướng mới có khác với hướng cuối cùng (lastDir) và không phải là hướng ngược lại (lastDir.Opposite())
                                                                      //Trả về true nếu có thể thay đổi hướng, ngược lại trả về false.
        }

        public void ChangeDirection(Direction dir) //Thay đổi hướng di chuyển của rắn nếu có thể
        {
            if (CanChangeDirection(dir)) //gọi phương thức CanChangeDirection và ktra xem hướng mới dir có thay đổi đc k
            {
                dirChanges.AddLast(dir); //Nếu CanChangeDirection trả về true, thêm hướng mới vào cuối danh sách dirChanges
            }
        }

        private bool OutsideGrid(Position pos) //ktra xem vị trí hiện tại của con rắn (pos) có nằm ngoài lưới ô hay không 
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols; // trả về true nếu pos.Row < 0 hoặc pos.Row >= số hàng (Rows)
                                                                                     // hoặc pos.Col < 0 hoặc pos.Col >= số cột (Cols)
        }

        private GridValue WillHit(Position newHeadPos) //Kiểm tra xem đầu rắn sẽ đụng phải gì khi di chuyển đến vị trí mới (newHeadPos)
        {
            if (OutsideGrid(newHeadPos)) //Nếu vị trí newHeadPos nằm ngoài lưới, trả về GridValue.Outside
            {
                return GridValue.Outside;
            }

            if (newHeadPos == TailPosition()) //Nếu vị trí newHeadPos bằng đuôi rắn, trả về GridValue.Empty
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col]; //Ngược lại, trả về giá trị của ô tại vị trí mới trong lưới (Grid)
        }

        public void Move() //Di chuyển rắn trong lưới, cập nhật vị trí, điểm số và trạng thái trò chơi
        {
            if (dirChanges.Count > 0) //ktra xem liệu hướng đi của rắn trong danh sách dirChanges có thay đổi hay không
            {
                Dir = dirChanges.First.Value; 
                dirChanges.RemoveFirst(); //Nếu có, cập nhật hướng hiện tại Dir bằng giá trị đầu tiên trong danh sách dirChanges
            }

            Position newHeadPos = HeadPosition().Translate(Dir); //Tạo vị trí đầu mới của rắn bằng cách dịch chuyển từ vị trí đầu hiện tại (HeadPosition()) theo hướng hiện tại (Dir)
            GridValue hit = WillHit(newHeadPos); //Kiểm tra rắn sẽ đụng phải gì khi di chuyển tới vị trí mới (newHeadPos) và lưu kết quả vào biến hit.

            if (hit == GridValue.Outside || hit == GridValue.Snake) //TH1: đụng phải tường or thân rắn => gameOver
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty) //TH2: đụng phải ô trống => di chuyển đuôi (loại bỏ gtri đuôi khỏi ô hiện tại) => thêm 1 ô đầu rắn 
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if (hit == GridValue.Food) //TH3: ăn được mồi => thêm 1 ô đầu rắn (không bỏ gtri ô đuôi rắn hiện tại vì rắn đang dài thêm)
                                            //& thêm score; thêm mồi random mới 
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
}
