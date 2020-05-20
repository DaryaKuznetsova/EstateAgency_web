namespace EstateAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int EstateObjectId { get; set; }

        public int? TradeId { get; set; }

        public virtual Client Client { get; set; }

        public virtual EstateObject EstateObject { get; set; }

        public virtual Trade Trade { get; set; }
    }
}
