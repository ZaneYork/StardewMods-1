﻿using System.Linq;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;

namespace AccessChestAnywhere
{
    /// <summary>The mod entry point.</summary>
    public class AccessChestAnywhere : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The selected chest.</summary>
        private Chest SelectedChest;


        /*********
        ** Public methods
        *********/
        /// <summary>Initialise the mod.</summary>
        public override void Entry(params object[] objects)
        {
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the player presses a key.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed != Keys.B || Game1.activeClickableMenu != null)
                return;

            // get chests
            ManagedChest[] chests = (
                from location in Game1.locations
                from obj in location.Objects.Where(p => p.Value is Chest)
                let chest = new ManagedChest((Chest)obj.Value, location, obj.Key)
                where !chest.IsIgnored
                orderby chest.Location.Name ascending, chest.Name ascending
                select chest
            ).ToArray();
            ManagedChest selectedChest = chests.FirstOrDefault(p => p.Chest == this.SelectedChest) ?? chests.First();

            // render menu
            if (chests.Any())
            {
                ACAMenu menu = new ACAMenu(chests, selectedChest);
                menu.OnChestSelected += chest => this.SelectedChest = chest.Chest; // remember selected chest on next load
                Game1.activeClickableMenu = menu;
            }
        }
    }
}