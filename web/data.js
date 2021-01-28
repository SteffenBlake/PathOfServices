class Carry {
    constructor(description, shorthand) {
        this._description = description;
        this._shorthand = shorthand;
    }
    get description() {
        return this._description;
    }
    get shorthand() {
        return this._shorthand;
    }
    get hasCurrency() {
        return false;
    }
    get hasCost() {
        return false;
    }
}

class Craft {
    constructor(description, shorthand, currency, cost) {
        this._description = description;
        this._shorthand = shorthand;
        this._currency = currency;
        this._cost = cost;
    }
    get description() {
        return this._description;
    }
    get shorthand() {
        return this._shorthand;
    }
    get currency() {
        return this._currency;
    }
    get cost() {
        return this._cost;
    }
    get hasCurrency() {
        return true;
    }
    get hasCost() {
        return true;
    }
}

class Horticraft {
    constructor(description, shorthand, currency) {
        this._description = description;
        this._shorthand = shorthand;
        this._currency = currency;
    }
    get description() {
        return this._description;
    }
    get shorthand() {
        return this._shorthand;
    }
    get currency() {
        return this._currency;
    }
    get hasCurrency() {
        return true;
    }
    get hasCost() {
        return false;
    }
}

export let Categories = ['Sellers', 'Buyers', 'Sales', 'Orders'];

export let Types = ['Crafts', 'Horticrafts', 'Carries'];

export let Carries = {
    'Bosses': [
        new Carry('Sirus', 'Sirus'),
        new Carry('Maven', 'Maven'),
        new Carry('Shaper', 'Shaper'),
        new Carry('Uber Elder', 'UE')
    ],
    'Trials': [
        new Carry('Trial of Piercing Truth', 'TrlPT'),
        new Carry('Trial of Swirling Fear', 'TrlSF'),
        new Carry('Trial of Crippling Grief', 'TrlCG'),
        new Carry('Trial of Burning Rage', 'TrlBR'),
        new Carry('Trial of Lingering Pain', 'TrlLP'),
        new Carry('Trial of Stinging Doubt', 'TrlSD')
    ],
    'Incursion': [
        new Carry('Doryani\'s Institute', 'IncDI'),
        new Carry('Apex of Ascension', 'IncAoA'),
        new Carry('Locus of Corruption', 'IncLoC'),
        new Carry('Throne of Atziri', 'IncToA'),
    ]
};

export let Crafts = {
    'Sockets': [
        new Craft('Six Sockets', '6S', 'J', '350'),
        new Craft('Six Links', '6L', 'F', '1500')
    ],
    'Weapons': [
        new Craft('% increased Spell Damage; Gain % of Non-Chaos Damage as extra Chaos Damage', 'SplDmg+NCAC', 'E', '1'),
        new Craft('Attacks with this Weapon Penetrate % Elemental Resistances', 'SplDmg+NCAC', 'E', '1'),
        new Craft('+2 to Level of Socketed Support Gems', '+2Sprt', 'E', '2'),
        new Craft('% to Quality of Socketed Gems', 'GemQ', 'E', '1'),
        new Craft('Hits can\'t be Evaded', 'Reso', 'E', '3'),
        new Craft('% increased Attack Speed;+% to Quality', 'SpdQ', 'E', '1'),
        new Craft('% increased Critical Strike Chance;+% to Quality', 'CritQ', 'E', '1'),
        new Craft('% increased Attack Speed;% chance to Trigger Level 1 Blood Rage when you Kill an Enemy', 'BldRg', 'E', '1'),
    ],
    'Flasks': [
        new Craft('Regenerate % of Life per second during Flask Effect', 'FlskRgn', 'G', '6'),
        new Craft('% Chance to avoid being Stunned during Flask Effect; % increased Movement Speed during Flask effect', 'FlskStnSpd', 'G', '6'),
        new Craft('% reduced Mana Cost of Skills during Flask Effect', 'FlskMana', 'G', '6'),
        new Craft('% increased Rarity of Items found during Flask Effect', 'FlskRar', 'G', '6'),
        new Craft('% increased Critical Strike Chance during Flask Effect', 'FlskCrit', 'G', '6'),
        new Craft('% of Damage Taken from Hits is Leeched as Life during Flask Effect', 'FlskLch', 'G', '6'),
    ],
    'Metacraft': [
        new Craft('Prefixes Cannot Be Changed', 'PrefIm', 'E', '2'),
        new Craft('Suffixes Cannot Be Changed', 'SufIm', 'E', '2'),
        new Craft('Can have up to 3 Crafted Modifiers', 'SufIm', 'E', '2'),
        new Craft('Cannot roll Attack Modifiers', 'CntAtt', 'E', '1'),
        new Craft('Cannot roll Caster Modifiers', 'CntCst', 'B', '5')
    ],
    'Other': [
        new Craft('(Belt) Flasks applied to you have % increased Effect', 'FlskEff', 'E', '1'),
        new Craft('(Belt) Flasks applied to you have % increased Effect; % Reduced Flask Charges Gained', 'FlskEffRedGn', 'E', '1'),
        new Craft('(Body) % increased Effect of your Curses', 'CrsEff', 'E', '1'),
        new Craft('(Ring) % increased Damage', 'Dmg', 'E', '1'),
        new Craft('(Gloves|Amulet) % increased Damage while Leeching', 'DmgLch', 'E', '1'),
        new Craft('(Helm) % of Physical Damage from Hits taken as Fire Damage', 'TknFire', 'E', '1'),
        new Craft('(Boots) Cannot be Frozen', 'CantFrz', 'E', '1')
    ]
};

let horticraftMods = {
    'Caster': 'Cst',
    'Physical': 'Phy',
    'Fire': 'Fir',
    'Attack': 'Att',
    'Life': 'Lif',
    'Cold': 'Cld',
    'Speed': 'Spd',
    'Defence': 'Def',
    'Lightning': 'Ltn',
    'Critical': 'Crt',
    'Influence': 'Inf'
};

let Horticrafts = {
    'Influence': [
        new Horticraft('Add a random Influence to a Normal, Magic or Rare Weapon that isn\'t influenced', 'WpnInf', 'W'),
        new Horticraft('Add a random Influence to a Normal, Magic or Rare Armour that isn\'t influenced', 'ArmInf', 'W'),
        new Horticraft('Add a random Influence to a Normal, Magic or Rare Jewellery that isn\'t influenced', 'JlrInf', 'W'),
    ],
    'Synthesis': [
        new Horticraft('Synthesise an item, giving random Synthesised implicits. Cannot be used on Unique, Influenced, Synthesised or Fractured items', 'Synth', 'S'),
        new Horticraft('Fracture a random modifier on an item with at least 5 modifiers, locking it in place. This can\'t be used on Influenced, Synthesised, or Fractured items', 'Frc5', 'S'),
        new Horticraft('Fracture a random Suffix on an item with at least 3 Suffixes. This can\'t be used on Influenced, Synthesised, or Fractured items', 'FrcSuf', 'S'),
        new Horticraft('Fracture a random Prefix on an item with at least 3 Prefixes. This can\'t be used on Influenced, Synthesised, or Fractured items.', 'FrcPre', 'S'),
    ],
};

for (const [key, value] of Object.entries(horticraftMods)) {
    Horticrafts[key] = Horticrafts[key] || [];
    Horticrafts[key].push(new Horticraft(`Remove a random non-${key} modifier from an item and add a new ${key} modifier`, `${value}Trans`, 'A'));
    Horticrafts[key].push(new Horticraft(`Augment a Magic or Rare item with a new ${key} modifier`, `${value}Aug`, 'E'));
    Horticrafts[key].push(new Horticraft(`Remove a random ${key} modifier from an item`, `${value}Rmv`, 'A'));
    Horticrafts[key].push(new Horticraft(`Randomise the numeric values of the random ${key} modifiers on a Magic or Rare item`, `${value}Div`, 'D'));
    Horticrafts[key].push(new Horticraft(`Remove a random ${key} modifier from an item and add a new ${key} modifier`, `${value}ReRoll`, 'E'));
    Horticrafts[key].push(new Horticraft(`Augment a Magic or Rare item with a new ${key} modifier with Lucky values`, `${value}AugLck`, 'E'));
}

export { Horticrafts }