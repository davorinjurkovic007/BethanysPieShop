using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShoo.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext appDbContext;

        public PieRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IEnumerable<Pie> AllPies
        {
            get
            {
                return appDbContext.Pies.Include(b => b.Category);
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return appDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek == true);
            }
        }

        public Pie GetPieById(int pieId)
        {
            return appDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }
    }
}
