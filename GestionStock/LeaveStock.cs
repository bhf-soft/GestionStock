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
    
    public partial class LeaveStock
    {
        public int Id { get; set; }
        public Nullable<int> Product_id { get; set; }
        public Nullable<int> Qte { get; set; }
        public Nullable<int> Client_id { get; set; }
        public Nullable<int> Users_id { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Product Product { get; set; }
        public virtual Users Users { get; set; }
    }
}
