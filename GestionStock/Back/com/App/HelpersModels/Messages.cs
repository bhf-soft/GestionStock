using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Back.com.App.HelpersModels
{
    class Messages
    {
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Messages(string value) { Value = value; }

        public string Value { get; set; }

        public static Messages DeleteAlerteText { get { return new Messages("Voulez-vous vraiment supprimer la(les) ligne(s) sélectionnée(s) ?"); } }

        public static Messages DeleteAlerteTitle { get { return new Messages("Attention"); } }

        public static Messages SelectLineAlerte { get { return new Messages("Veuillez sélectionnez une ligne"); } }


        public static Messages ClientAdded { get { return new Messages("Client Ajoutée"); } }
        public static Messages ClientModified { get { return new Messages("Client Modifiée"); } }
        public static Messages ClientDeleted { get { return new Messages("Client Supprimée"); } }

        public static Messages ProviderAdded { get { return new Messages("Fournisseur Ajoutée"); } }
        public static Messages ProviderModified { get { return new Messages("Fournisseur Modifiée"); } }
        public static Messages ProviderDeleted { get { return new Messages("Fournisseur Supprimée"); } }

        public static Messages CategoryAdded { get { return new Messages("Catégorie Ajoutée"); } }
        public static Messages CategoryModified { get { return new Messages("Catégorie Modifiée"); } }
        public static Messages CategoryDeleted { get { return new Messages("Catégorie Supprimée"); } }

        public static Messages ProductAdded { get { return new Messages("Catégorie Ajoutée"); } }
        public static Messages ProductModified { get { return new Messages("Catégorie Modifiée"); } }
        public static Messages ProductDeleted { get { return new Messages("Catégorie Supprimée"); } }

        public static Messages ProviderCommandAdded { get { return new Messages("Commande Fournisseur Ajoutée"); } }
        public static Messages ProviderCommandModified { get { return new Messages("Commande Fournisseur Modifiée"); } }
        public static Messages ProviderCommandDeleted { get { return new Messages("Commande Fournisseur Supprimée"); } }

        public static Messages ProviderCommandBillAdded { get { return new Messages("Facture Fournisseur Ajoutée"); } }

    }
}
