using System;
using System.Collections.Generic;

namespace Snake
{
    public class Direction
    {
        // Định nghĩa các hướng di chuyển tĩnh (static) với các giá trị cụ thể cho rowOffset và colOffset.
        // Các giá trị này xác định hướng di chuyển của rắn trên bảng (trái, phải, lên, xuống)
        public readonly static Direction Left = new Direction(0, -1); //khi rắn di chuyển sang trái, giữ nguyên gtri hàng, thay đổi gtri cột sang trái (-1)
        public readonly static Direction Right = new Direction(0, 1); //khi rắn di chuyển sang trái, giữ nguyên gtri hàng, thay đổi gtri cột sang phải (+1)
        public readonly static Direction Up = new Direction(-1, 0); //khi rắn di chuyển lên trên, giữ nguyên gtri cột, thay đổi gtri hàng.
                                                                    //ban đầu, bắt đầu mảng 2 chiều tạo thành lưới ô (grid), ô bắt đầu nằm ở góc trên bên trái là (0;0) xem như gốc tọa độ,
                                                                    //điều đó nghĩa là con rắn càng đi lên trên thì số hàng càng giảm dần (tiến gần về gốc tọa độ) => gtri hàng -1
        public readonly static Direction Down = new Direction(1, 0); //tương tự như trên

        public int RowOffset { get; } //Định nghĩa một thuộc tính công khai chỉ đọc RowOffset để lưu trữ giá trị di chuyển theo hàng
        public int ColOffset { get; } //tương tự

        private Direction(int rowOffset, int colOffset) // hàm gán gtri rowOffset và colOffset cho đối tượng
        {
            RowOffset = rowOffset; 
            ColOffset = colOffset;
        }

        public Direction Opposite() //phương thức Opposite để trả về hướng ngược lại của một đối tượng Direction.
                                    //Nó trả về một đối tượng Direction mới với giá trị rowOffset và colOffset bị đảo ngược.
        {
            return new Direction(-RowOffset, -ColOffset);
        }

        public override bool Equals(object obj) //override là từ khóa ghi đè; được sử dụng khi muốn thay đổi hành vi của một phương thức (method) ở lớp cha(base class) trong lớp con(derived class);
                                                //Ghi đè phương thức Equals để so sánh hai đối tượng Direction.
                                                //Phương thức này kiểm tra xem đối tượng đang được so sánh có phải là một Direction và có các giá trị rowOffset và colOffset bằng nhau hay không.
        {
            return obj is Direction direction && 
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;
        }

        public override int GetHashCode() //chỗ này em chưa hiểu
        {
            return HashCode.Combine(RowOffset, ColOffset);
        }

        public static bool operator ==(Direction left, Direction right) //"==" so sánh 2 đối tượng direction, xem chúng có giống nhau không
                                                                        //sử dụng EqualityComparer để kiểm tra xem hai đối tượng có bằng nhau hay không;
                                                                        //Khi hai đối tượng Direction có cùng RowOffset và ColOffset, phép so sánh == sẽ trả về true, nếu khác nhau, kết quả sẽ là false
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right) //so sánh hai đối tượng Direction để kiểm tra xem chúng có khác nhau không
        {
            return !(left == right);
        }
    }
}
