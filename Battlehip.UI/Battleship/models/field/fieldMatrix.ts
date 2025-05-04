export type FieldMatrix = {
    Items: CellType[][]
}

export enum CellType{
    Ship = 1,
    Empty,
    Forbidden,
    DeadShip
}