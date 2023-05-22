using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Feature {
    public enum FeatureType {
        CriminalContact,
        CriminalSpecialty,
        Darkvision,
        DwarvenCombatTraining,
        DwarvenResilience,
        FalseIdentity,
        FavoriteSchemes,
        FeyAncestry,
        KeenSenses,
        MilitaryRank,
        ShelterOfTheFaithfull,
        Specialty,
        Stonecutting,
        Trance,
        WatchersEye
    }

    public string featureTitle;
    //public FeatureType featureType;
    public string featureDescription;

    public Feature(string featureTitle, string featureDescription) {
        this.featureTitle = featureTitle;
        this.featureDescription = featureDescription;
    }
}
