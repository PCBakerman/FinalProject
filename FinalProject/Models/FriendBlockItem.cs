using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace FinalProject.Models
{
    public class FriendBlockItem
    {
        [Key, Column(Order = 0)]
        public int UserInventoryOneId { get; set; }
        public UserInventory UserInventoryOne { get; set; }
        [Key, Column(Order = 1)]
        public int UserInventorTwoId { get; set; }
        public UserInventory UserInventoryTwo { get; set; }
        public FriendBlockStatus Status { get; set; }
    }
    public enum FriendBlockStatus
    {
        Neutral,
        Friends,
        Blocked,
        Pending
    }
}
