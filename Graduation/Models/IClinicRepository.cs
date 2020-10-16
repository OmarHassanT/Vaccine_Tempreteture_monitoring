using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public interface IClinicRepository
    {
        Clinic GetClinic(int id);
        IEnumerable<Clinic> GetClinics();
        Clinic AddClinc(Clinic clinic);
        Clinic Delete(int id);
        Clinic Update(Clinic clinic);
        Task<IEnumerable<fridgeTempreture>> getTempretures(int ClinId, int pageSize,int pageIndex);
        IEnumerable<Clinic> GetClinicsFromDirectorate(int directorateId);

    }
}
