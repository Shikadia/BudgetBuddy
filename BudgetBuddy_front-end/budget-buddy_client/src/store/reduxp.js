import { legacy_createStore as creatstore, applyMiddleware } from "redux";
import { combineReducers } from "redux";
import { thunk } from "redux-thunk";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage";


const authReducer = (state = {user: null, id: null, token: null, refreshToken: null}, action) => {
    switch (action.type) {
        case 'login':
            return {...state, user: action.payload.user, id: action.payload.id, token: action.payload.token, refreshToken: action.payload.refreshToken};
        case 'logout':
            return {...state, user: null, id: null, token: null, refreshToken: null};
        default:
            return state;
    }
};

const rootReducer = combineReducers({
    auth: authReducer,
});

const persistConfig = {
    key: 'root',
    storage,
    whiteList: ['auth'],
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

export const store = creatstore(persistedReducer, applyMiddleware(thunk));
export const persistor = persistStore(store);