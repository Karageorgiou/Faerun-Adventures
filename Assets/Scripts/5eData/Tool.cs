using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tool {
    public enum ToolProficiency {
        None,
        // Artisan Tools
        AlchemistSupplies,
        BrewerSupplies,
        CaligrapherSupplies,
        CarpenterTools,
        CartographerTools,
        CobblerTools,
        CookUtensils,
        GlassblowerTools,
        JewelerTools,
        LeatherworkerTools,
        MasonTools,
        PainterSupplies,
        PotterTools,
        SmithTools,
        TinkerTools,
        WeaverTools,
        WoodcarverTools,
        // Miscellaneous
        DisguiseKit,
        ForgeryKit,
        HerbalismKit,
        NavigatorTools,
        PoisonerKit,
        ThievesTools
    }


    public enum ArtisanTool {
        AlchemistSupplies,
        BrewerSupplies,
        CaligrapherSupplies,
        CarpenterTools,
        CartographerTools,
        CobblerTools,
        CookUtensils,
        GlassblowerTools,
        JewelerTools,
        LeatherworkerTools,
        MasonTools,
        PainterSupplies,
        PotterTools,
        SmithTools,
        TinkerTools,
        WeaverTools,
        WoodcarverTools
    }

    public enum GamingSet {
        None,
        //todo
    }

    public enum MusicalInstrument {
        None,
        //todo
    }

    public enum Miscellaneous {
        DisguiseKit,
        ForgeryKit,
        HerbalismKit,
        NavigatorTools,
        PoisonerKit,
        ThievesTools
    }
}
