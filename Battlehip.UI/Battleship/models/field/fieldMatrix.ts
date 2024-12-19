export type FieldMatrix = {
    Items: CellType[][]
}

export enum CellType{
    Empty = 1,
    Ship,
    Forbidden
}