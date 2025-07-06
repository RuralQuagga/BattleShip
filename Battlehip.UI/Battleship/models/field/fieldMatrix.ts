export type FieldMatrix = {
  Items: CellType[][];
};

export enum CellType {
  Ship = 1,
  Empty,
  Forbidden,
  DeadShip,
  Miss,
  ForbiddenMiss,
}

export enum ActionState {
  Success = 1,
  Fail,
  Win,
  Lose,
}

export interface FieldDto {
  fieldId: string;
  sessionId: string;
  isPlayerField: boolean;
  fieldConfiguration: CellType[][];
}

export interface CheckCellApiRequest {
  fieldId: string;
  line: number;
  cell: number;
}

export interface CheckCellApiResponse {
  field: FieldDto;
  isSuccessCheck: ActionState;
}

export interface FieldReducerState {
  userFieldId: string;
  computerFieldId: string;
}

export interface GameStatistic {
  gameTimeMs: number;
  yourMoves: number;
  enemyMoves: number;
  hitPercentage: number;
}
