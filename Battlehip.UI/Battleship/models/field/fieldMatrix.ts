export type FieldMatrix = {
    Items: CellType[][]
}

export enum CellType{
    Ship = 1,
    Empty,
    Forbidden,
    DeadShip,
    Miss,
    ForbiddenMiss
}

export interface FieldDto {
    fieldId: string,
    sessionId: string,
    isPlayerField: boolean,
    fieldConfiguration: CellType[][]
}

export interface CheckCellApiRequest{
    fieldId: string,
    line: number,
    cell: number
}