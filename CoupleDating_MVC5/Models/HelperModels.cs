using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoupleDating_MVC5.Models
{
    public class HelperModels
    {
        public IEnumerable<KeyValuePair<int, int>> GetAllYears(int startYear)
        {
            for (int i = startYear; i <= DateTime.Now.Year; i++)
            {
                yield return new KeyValuePair<int, int>(i, i);
            }
        }
    }
}