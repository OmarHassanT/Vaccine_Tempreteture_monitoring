using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class SqlDirectorateRepositery : IDirectorateRepository
    {
        private readonly AppDBContext context;

        public SqlDirectorateRepositery(AppDBContext context)
        {
            this.context = context;
        }
       
        public Directorate AddDirectorate(Directorate directorate)
        {
            context.Directorates.Add(directorate);
            context.SaveChanges();
            return directorate;
        }

        public IEnumerable<Clinic> GetClinics(int DirectorateId)
        {
            IEnumerable<Clinic> Clinics = context.Clinics.Where(e => e.DirectorateId == DirectorateId);
            return Clinics;
        }



        public Directorate Delete(int id)
        {
            Directorate directorate= context.Directorates.Find(id);
            if(directorate !=null)
            {
                context.Directorates.Remove(directorate);
                context.SaveChanges();
            }
            return directorate;
        }

        public Directorate GetDirectorate(int? id)
        {
          return context.Directorates.FirstOrDefault(e=>e.DirectorateId==id);
           

        }

        public IEnumerable<Directorate> GetDirectorates()
        {
           return context.Directorates;
        }

        public Directorate Update(Directorate directorate)
        {
            var newDirectorate= context.Directorates.Attach(directorate);
            newDirectorate.State= Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return directorate;
        }
    }
}
