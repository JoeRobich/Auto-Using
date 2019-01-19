export function isWhitespace(char: string): boolean {
    return /\s/.test(char) || char === "";
}

export const SORT_CHEAT = "\u200B";

export const syntaxChars = ["{", "}", "(", ")", "[", "]", "<", ">", "@", ";", "=", "%", "&", "*", "+", ",", "-", "/", ":", "?", "^", "|"];

export const showSuggestFor = ["abstract", "new", "protected", "return", "sizeof", "struct", "using", "volatile", "as",
	"checked", "explicit", "fixed", "goto", "lock", "override", "public", "stackalloc", "unchecked",
	"static", "base", "case", "else", "extern", "if", "params", "readonly", "sealed", "static", "typeof", "unsafe", "virtual", "const", "implicit",
	"internal", "private", "await", "this", "in"
];


export const primitives = {
	bool: "Boolean", byte: "Byte", sbyte: "SByte", char: "Char", decimal: "Decimal",
	double: "Double", float: "Single", int: "Int32", uint: "UInt32", long: "Int64", ulong: "System.UInt64",
	object: "Object", short: "Int16", ushort: "Uint16", string: "String"
};