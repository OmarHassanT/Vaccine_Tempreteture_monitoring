using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public interface IDirectorateRepository
    {
        Directorate GetDirectorate(int? id);
        Directorate AddDirectorate(Directorate directorate);
        IEnumerable<Directorate> GetDirectorates();
        Directorate Update(Directorate directorate);
        Directorate Delete(int id);
        IEnumerable<Clinic> GetClinics(int DirectorateId);

    }
}
