grammar LabCalculator;

compileUnit : expression EOF;

expression : logicEqv;

// Пріоритети
logicEqv : logicOr (EQV logicOr)*;
logicOr : logicAnd (OR logicAnd)*;
logicAnd : comparison (AND comparison)*;
comparison : additive ( (EQ | LT | GT) additive )?;

// --- ОСЬ ТУТ БУЛА ПРОБЛЕМА ---
// Ми додали мітки # BinaryAddSub та # UnaryExpr
// Без них C# не знає, що це таке.
additive 
    : additive (PLUS | MINUS) additive  # BinaryAddSub
    | unary                             # UnaryExpr
    ;

unary 
    : (PLUS | MINUS) unary    # UnaryNum
    | NOT unary               # UnaryBool
    | atom                    # AtomExpr
    ;

atom 
    : NUMBER                  # Constant
    | IDENTIFIER              # CellRef
    | TRUE                    # BoolTrue
    | FALSE                   # BoolFalse
    | LPAREN expression RPAREN # ParenExpr
    ;

// Лексер
EQV: 'eqv';
OR: 'or';
AND: 'and';
NOT: 'not';
EQ: '=';
LT: '<';
GT: '>';
PLUS: '+';
MINUS: '-';
LPAREN: '(';
RPAREN: ')';
TRUE: 'true';
FALSE: 'false';

NUMBER: [0-9]+;
IDENTIFIER: [a-zA-Z]+[0-9]+;
WS: [ \t\r\n]+ -> skip;