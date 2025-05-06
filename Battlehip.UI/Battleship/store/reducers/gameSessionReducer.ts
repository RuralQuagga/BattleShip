import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { SessionId } from "../../models/gameSession/gameSessionTypes";

const initialState: SessionId = { sessionId: "" }
const SessionReducer = createSlice({
    name: 'SessionId',
    initialState,
    reducers: {
        addNew: (state, action: PayloadAction<string>) => {
            state.sessionId = action.payload;
        },
    },
})

export const { addNew } = SessionReducer.actions;
export default SessionReducer.reducer;