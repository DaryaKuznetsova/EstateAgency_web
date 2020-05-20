namespace EstateAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trade")]
    public partial class Trade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trade()
        {
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }

        public int EstateObjectId { get; set; }

        public int ManagerId { get; set; }

        public int ClientId { get; set; }

        public DateTime Date { get; set; }

        public int? PaymentTypeId { get; set; }

        public int? PaymentInstrumentId { get; set; }

        public virtual Client Client { get; set; }

        public virtual Manager Manager { get; set; }

        public virtual PaymentInstrument PaymentInstrument { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
    }
}
