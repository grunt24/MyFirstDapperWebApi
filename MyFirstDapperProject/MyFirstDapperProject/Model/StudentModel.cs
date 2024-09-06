using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstDapperProject.Model
{
    [Table("StudentTable")]
    public class StudentModel
    {
        [Key]
        public int StudentId { get; set; }
        [Column("First_Name")]
        public string FirstName { get; set; }
        [Column("Last_Name")]
        public string LastName { get; set; }

    }
}