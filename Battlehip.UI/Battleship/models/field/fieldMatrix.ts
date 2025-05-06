export type FieldMatrix = {
    Items: CellType[][]
}

export enum CellType{
    Ship = 1,
    Empty,
    Forbidden,
    DeadShip
}

export interface FieldDto {
    fieldId: string,
    sessionId: string,
    isPlayerField: boolean,
    fieldConfiguration: CellType[][]
}