using System;
using System.Collections.Generic;

namespace Snake
{
    public class Direction // Định nghĩa lớp Direction để biểu diễn hướng di chuyển.
    {
        // Định nghĩa các hướng tĩnh (static) cho Left, Right, Up và Down.
        public readonly static Direction Left = new Direction(0, -1); // Hướng trái với offset hàng là 0 và cột là -1.
        public readonly static Direction Right = new Direction(0, 1); // Hướng phải với offset hàng là 0 và cột là 1.
        public readonly static Direction Up = new Direction(-1, 0); // Hướng lên với offset hàng là -1 và cột là 0.
        public readonly static Direction Down = new Direction(1, 0); // Hướng xuống với offset hàng là 1 và cột là 0.

        public int RowOffset { get; } // Thuộc tính chỉ đọc để lấy offset hàng.
        public int ColOffset { get; } // Thuộc tính chỉ đọc để lấy offset cột.

        private Direction(int rowOffset, int colOffset) // Constructor riêng tư để khởi tạo đối tượng Direction.
        {
            RowOffset = rowOffset; // Gán giá trị offset hàng.
            ColOffset = colOffset; // Gán giá trị offset cột.
        }

        public Direction Opposite() // Phương thức để lấy hướng ngược lại.
        {
            return new Direction(-RowOffset, -ColOffset); // Trả về đối tượng Direction mới với offset ngược lại.
        }

        public override bool Equals(object obj) // Ghi đè phương thức Equals để so sánh hai đối tượng Direction.
        {
            return obj is Direction direction && // Kiểm tra xem obj có phải là Direction không và
                   RowOffset == direction.RowOffset && // so sánh offset hàng
                   ColOffset == direction.ColOffset; // và offset cột.
        }

        public override int GetHashCode() // Ghi đè phương thức GetHashCode để lấy mã băm cho đối tượng Direction.
        {
            return HashCode.Combine(RowOffset, ColOffset); // Kết hợp mã băm của offset hàng và cột.
        }

        public static bool operator ==(Direction left, Direction right) // Định nghĩa toán tử == để so sánh hai đối tượng Direction.
        {
            return EqualityComparer<Direction>.Default.Equals(left, right); // Sử dụng EqualityComparer để so sánh.
        }

        public static bool operator !=(Direction left, Direction right) // Định nghĩa toán tử != để so sánh hai đối tượng Direction.
        {
            return !(left == right); // Trả về kết quả ngược lại của toán tử ==.
        }
    }
}
