//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestionStock
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientBill
    {
        public int Id { get; set; }
        public Nullable<int> ClientCommand_id { get; set; }
        public string PurchaseMethod { get; set; }
        public Nullable<int> Users_id { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public string PurchaseNumber { get; set; }
        public Nullable<float> Total { get; set; }
        public Nullable<float> Rest { get; set; }
    
        public virtual ClientCommand ClientCommand { get; set; }
        public virtual Users Users { get; set; }
    }
}
