export interface Session {
  id: string;
  sessionStart: Date | null;
  sessionEnd: Date | null;
  state: SessionState | null;
}

export enum SessionState {
  Preparing = 1,
  InProgress,
  Win,
  Loss,
  Closed,
}
