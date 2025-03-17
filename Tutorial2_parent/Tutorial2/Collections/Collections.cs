using Tutorial2.Collections.Data;

namespace Tutorial2.Collections;

public class Collections
{
   public static List<StudentResult> ProcessGrades(List<Student> students)
   {
      var results = new List<StudentResult>();
      foreach (var stud in students)
      {
         string result = "";
         if (stud.Grade >= 90)
         {
            result = "Exemption";
         }

         if (stud.Grade >= 50 && stud.Grade < 90)
         {
            result = "Passing";
         }

         if (stud.Grade < 50)
         {
            result = "Needs Improvement";
         }
         
         results.Add(new StudentResult()
         {
            Name = stud.Name,
            Result = result
         });
      }
      return results;
   }
}