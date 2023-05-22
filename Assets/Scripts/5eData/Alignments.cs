using System;

namespace DnD5eData {

    public static class Alignments {

        public enum LawVsChaos {
            None,
            Lawful,
            Neutral,
            Chaotic
        }

        public enum GoodVsEvil {
            None,
            Good,
            Neutral,
            Evil
        }

        [Serializable]
        public struct Alignment {
            public LawVsChaos LawVsChaos;
            public GoodVsEvil GoodVsEvil;

            public Alignment(LawVsChaos lawVsChaos, GoodVsEvil goodVsEvil) {
                LawVsChaos = lawVsChaos;
                GoodVsEvil = goodVsEvil;
            }
        }
    }
}

