using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace StudentManagementApp
{
    class Program
    {
        private static readonly string FilePath = "students.xml";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int choice = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("===== QUẢN LÝ SINH VIÊN =====");
                Console.WriteLine("1. Hiển thị danh sách sinh viên");
                Console.WriteLine("2. Sinh viên GPA >= 8");
                Console.WriteLine("3. Sinh viên theo lớp");
                Console.WriteLine("4. Danh sách sinh viên nữ");
                Console.WriteLine("5. Sinh viên GPA cao nhất");
                Console.WriteLine("6. Sinh viên GPA thấp nhất");
                Console.WriteLine("7. Đếm số sinh viên");
                Console.WriteLine("8. GPA trung bình");
                Console.WriteLine("9. Thống kê theo lớp");
                Console.WriteLine("10. Sắp xếp theo GPA");
                Console.WriteLine("11. Tìm kiếm sinh viên theo độ tuổi (20-21)");
                Console.WriteLine("12. Sinh viên CNTT và GPA >= 8.0");
                Console.WriteLine("13. Tìm kiếm theo mã sinh viên");
                Console.WriteLine("14. Tìm kiếm theo lớp và sắp xếp GPA giảm dần");
                Console.WriteLine("15. Top 3 sinh viên GPA cao nhất");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Lựa chọn không hợp lệ! Vui lòng nhập số.");
                    PressAnyKeyToContinue();
                    continue;
                }

                // Xử lý kiểm tra file XML tồn tại hoặc lỗi đọc file theo yêu cầu kỹ thuật[cite: 1]
                if (!File.Exists(FilePath))
                {
                    Console.WriteLine($"\nLỗi: Không tìm thấy file '{FilePath}'. Vui lòng tạo file XML trước khi chạy chức năng.");
                    PressAnyKeyToContinue();
                    continue;
                }

                try
                {
                    // Sử dụng XDocument để đọc dữ liệu XML[cite: 1]
                    XDocument doc = XDocument.Load(FilePath);

                    switch (choice)
                    {
                        case 1:
                            DisplayAll(doc);
                            break;
                        case 2:
                            DisplayByGPA(doc, 8.0);
                            break;
                        case 3:
                            DisplayByClass(doc, "CTK44");
                            break;
                        case 4:
                            DisplayByGender(doc, "Nu");
                            break;
                        case 5:
                            DisplayMaxGPA(doc);
                            break;
                        case 6:
                            DisplayMinGPA(doc);
                            break;
                        case 7:
                            CountStudents(doc);
                            break;
                        case 8:
                            AverageGPA(doc);
                            break;
                        case 9:
                            StatisticsByClass(doc);
                            break;
                        case 10:
                            SortByGPA(doc);
                            break;
                        case 11:
                            DisplayByAgeRange(doc, 20, 21);
                            break;
                        case 12:
                            DisplayMajorAndGPA(doc, "Cong nghe thong tin", 8.0);
                            break;
                        case 13:
                            SearchById(doc);
                            break;
                        case 14:
                            SearchClassAndSortGPA(doc);
                            break;
                        case 15:
                            Top3HighestGPA(doc);
                            break;
                        case 0:
                            Console.WriteLine("\nĐã thoát chương trình. Tạm biệt!");
                            break;
                        default:
                            Console.WriteLine("\nChức năng không hợp lệ!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi khi đọc file XML bị hỏng hoặc lỗi phát sinh[cite: 1]
                    Console.WriteLine($"\nLỗi đọc file XML: {ex.Message}");
                }

                if (choice != 0)
                {
                    PressAnyKeyToContinue();
                }

            } while (choice != 0);
        }

        private static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }

        private static void PrintHeader()
        {
            Console.WriteLine($"{"Mã SV",-8} | {"Họ Tên",-20} | {"Giới tính",-9} | {"Tuổi",-4} | {"Lớp",-6} | {"Chuyên ngành",-22} | {"GPA",-4}");
            Console.WriteLine(new string('-', 85));
        }

        private static void PrintRow(XElement s)
        {
            string id = s.Attribute("id")?.Value ?? "";
            string name = s.Element("Name")?.Value ?? "";
            string gender = s.Element("Gender")?.Value ?? "";
            string age = s.Element("Age")?.Value ?? "";
            string cls = s.Element("Class")?.Value ?? "";
            string major = s.Element("Major")?.Value ?? "";
            string gpa = s.Element("GPA")?.Value ?? "";

            Console.WriteLine($"{id,-8} | {name,-20} | {gender,-9} | {age,-4} | {cls,-6} | {major,-22} | {gpa,-4}");
        }

        // Câu 1: Hiển thị danh sách tất cả sinh viên bằng LINQ to XML[cite: 1]
        private static void DisplayAll(XDocument doc)
        {
            Console.WriteLine("\n--- DANH SÁCH TẤT CẢ SINH VIÊN ---");
            var query = doc.Descendants("Student");
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 2: Sinh viên GPA >= 8.0[cite: 1]
        private static void DisplayByGPA(XDocument doc, double threshold)
        {
            Console.WriteLine($"\n--- DANH SÁCH SINH VIÊN CÓ GPA >= {threshold} ---");
            var query = doc.Descendants("Student")
                           .Where(s => (double)s.Element("GPA") >= threshold);
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 3: Sinh viên theo lớp[cite: 1]
        private static void DisplayByClass(XDocument doc, string targetClass)
        {
            Console.WriteLine($"\n--- DANH SÁCH SINH VIÊN LỚP {targetClass} ---");
            var query = doc.Descendants("Student")
                           .Where(s => (string)s.Element("Class") == targetClass);
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 4: Danh sách sinh viên nữ[cite: 1]
        private static void DisplayByGender(XDocument doc, string targetGender)
        {
            Console.WriteLine("\n--- DANH SÁCH SINH VIÊN NỮ ---");
            var query = doc.Descendants("Student")
                           .Where(s => (string)s.Element("Gender") == targetGender);
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 5: Sinh viên GPA cao nhất[cite: 1]
        private static void DisplayMaxGPA(XDocument doc)
        {
            Console.WriteLine("\n--- SINH VIÊN CÓ GPA CAO NHẤT ---");
            var students = doc.Descendants("Student");
            if (!students.Any()) return;
            double max = students.Max(s => (double)s.Element("GPA"));
            var query = students.Where(s => (double)s.Element("GPA") == max);
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 6: Sinh viên GPA thấp nhất[cite: 1]
        private static void DisplayMinGPA(XDocument doc)
        {
            Console.WriteLine("\n--- SINH VIÊN CÓ GPA THẤP NHẤT ---");
            var students = doc.Descendants("Student");
            if (!students.Any()) return;
            double min = students.Min(s => (double)s.Element("GPA"));
            var query = students.Where(s => (double)s.Element("GPA") == min);
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 7: Đếm số sinh viên[cite: 1]
        private static void CountStudents(XDocument doc)
        {
            int count = doc.Descendants("Student").Count();
            Console.WriteLine($"\nTổng số lượng sinh viên: {count}");
        }

        // Câu 8: GPA trung bình[cite: 1]
        private static void AverageGPA(XDocument doc)
        {
            var students = doc.Descendants("Student");
            if (!students.Any()) return;
            double avg = students.Average(s => (double)s.Element("GPA"));
            Console.WriteLine($"\nGPA trung bình của tất cả sinh viên: {avg:F2}");
        }

        // Câu 9: Thống kê theo lớp[cite: 1]
        private static void StatisticsByClass(XDocument doc)
        {
            Console.WriteLine("\n--- THỐNG KÊ SỐ LƯỢNG SINH VIÊN THEO LỚP ---");
            var query = doc.Descendants("Student")
                           .GroupBy(s => (string)s.Element("Class"));
            foreach (var group in query)
            {
                Console.Write($"{group.Key}: {group.Count()} sinh viên   ");
            }
            Console.WriteLine();
        }

        // Câu 10: Sắp xếp theo GPA giảm dần[cite: 1]
        private static void SortByGPA(XDocument doc)
        {
            Console.WriteLine("\n--- DANH SÁCH SINH VIÊN SẮP XẾP GPA GIẢM DẦN ---");
            var query = doc.Descendants("Student")
                           .OrderByDescending(s => (double)s.Element("GPA"));
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 11: Tìm kiếm theo độ tuổi[cite: 1]
        private static void DisplayByAgeRange(XDocument doc, int minAge, int maxAge)
        {
            Console.WriteLine($"\n--- SINH VIÊN CÓ TUỔI TỪ {minAge} ĐẾN {maxAge} ---");
            var query = doc.Descendants("Student")
                           .Where(s => {
                               int age = (int)s.Element("Age");
                               return age >= minAge && age <= maxAge;
                           });
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 12: Sinh viên CNTT và GPA >= 8.0[cite: 1]
        private static void DisplayMajorAndGPA(XDocument doc, string major, double minGpa)
        {
            Console.WriteLine($"\n--- SINH VIÊN NGÀNH {major} VÀ GPA >= {minGpa} ---");
            var query = doc.Descendants("Student")
                           .Where(s => (string)s.Element("Major") == major && (double)s.Element("GPA") >= minGpa);
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }

        // Câu 13: Tìm kiếm theo mã sinh viên[cite: 1]
        private static void SearchById(XDocument doc)
        {
            Console.Write("\nNhập mã sinh viên cần tìm (VD: SV001): ");
            string id = Console.ReadLine()?.Trim();

            var student = doc.Descendants("Student")
                             .FirstOrDefault(s => (string)s.Attribute("id") == id);

            if (student == null)
            {
                Console.WriteLine("Không tìm thấy sinh viên.");
            }
            else
            {
                Console.WriteLine("\n--- KẾT QUẢ TÌM KIẾM ---");
                PrintHeader();
                PrintRow(student);
            }
        }

        // Câu 14: Tìm kiếm theo lớp và sắp xếp GPA giảm dần[cite: 1]
        private static void SearchClassAndSortGPA(XDocument doc)
        {
            Console.Write("\nNhập tên lớp cần tìm (VD: CTK44): ");
            string className = Console.ReadLine()?.Trim();

            var query = doc.Descendants("Student")
                           .Where(s => (string)s.Element("Class") == className)
                           .OrderByDescending(s => (double)s.Element("GPA"));

            if (!query.Any())
            {
                Console.WriteLine($"Không tìm thấy sinh viên nào thuộc lớp {className}.");
            }
            else
            {
                Console.WriteLine($"\n--- SINH VIÊN LỚP {className} (SẮP XẾP GPA GIẢM DẦN) ---");
                PrintHeader();
                foreach (var s in query) PrintRow(s);
            }
        }

        // Câu 15: Top 3 sinh viên GPA cao nhất[cite: 1]
        private static void Top3HighestGPA(XDocument doc)
        {
            Console.WriteLine("\n--- TOP 3 SINH VIÊN CÓ GPA CAO NHẤT ---");
            var query = doc.Descendants("Student")
                           .OrderByDescending(s => (double)s.Element("GPA"))
                           .Take(3);
            PrintHeader();
            foreach (var s in query) PrintRow(s);
        }
    }
}