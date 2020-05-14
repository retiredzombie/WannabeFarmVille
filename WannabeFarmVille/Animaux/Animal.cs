using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannabeFarmVille
{
    abstract class Animal
    {
        public abstract int getGestation();
        public abstract int getCroissance();
        public abstract int getFaim();
        public abstract int getGenre();
    }
}
