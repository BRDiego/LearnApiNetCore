using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiNetCore.Business.Models.ManyToMany
{
    public class BandMusician
    {
        public ushort BandId { get; set; }
        public ushort MusicianId { get; set; }
        public Band Band { get; set; } = null!;
        public Musician Musician { get; set; } = null!;
    }
}
