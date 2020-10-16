using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class SqlClinicRepository : IClinicRepository
    {
        private readonly AppDBContext context;

        public SqlClinicRepository(AppDBContext context)
        {
            this.context = context;
        }
        public Clinic AddClinc(Clinic clinic)
        {
            context.Clinics.Add(clinic);
            context.SaveChanges();
            return clinic;
        }

        public Clinic Delete(int id)
        {
          var clinic=  context.Clinics.Find(id);
            if (clinic != null)
            {
                context.Clinics.Remove(clinic);
                context.SaveChanges();

            }
            return clinic;
        }

        public Clinic GetClinic(int id)
        {
           return context.Clinics.FirstOrDefault(e => e.clinicId == id);
        }

        public IEnumerable<Clinic> GetClinics()
        {
            return context.Clinics;
        }

        public Clinic Update(Clinic clinic)
        {
         var newClinic=context.Clinics.Attach(clinic);
            newClinic.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return clinic;
        }
        public async Task<IEnumerable<fridgeTempreture>> getTempretures(int ClinId,int pageSize,int pageIndex)
        {
            var query= context.fridgeTempretures.AsNoTracking().Where(e => e.clinicId == ClinId).OrderByDescending(e=>e.id);
            var model =await PagingList.CreateAsync<fridgeTempreture>(query, pageSize, pageIndex);
            return model;
        }

        public IEnumerable<Clinic> GetClinicsFromDirectorate(int directorateId)
        {
          return  context.Clinics.Where(e => e.DirectorateId == directorateId);
        }
    }
}
