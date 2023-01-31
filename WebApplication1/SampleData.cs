using System.Linq;
using System.Text;
using System.Drawing;
using WebApplication1.Models;

namespace WebApplication1
{
    public class SampleData
    {
        public static void Initialize(ApplicationContext context)
        {
            if (!context.Films.Any())
            {
                context.Films.AddRange(
                    new Film
                    {
                        Name = "Film1",
                        Description = "123",
                        Rating = 5
                    },
                    new Film
                    {
                        Name = "Film2",
                        Description = "1234"
                    },
                    new Film
                    {
                        Name = "Film3",
                        Description = "12345"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
