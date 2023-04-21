using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal interface IEnnemi
{
    // Permet d'activer l'ennemi.
    public void ActiverEnnemi();
    // Perme de voir si l'ennemi est encore actif. Fait passer au suivant.
    public bool EnnemiEstActif();

}

