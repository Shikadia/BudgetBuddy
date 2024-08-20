import { legacy_createStore as creatstore, applyMiddleware } from "redux";
import { combineReducers } from "redux";
import { thunk } from "redux-thunk";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage";

const authReducer = (
  state = { user: null, id: null, token: null, refreshToken: null },
  action
) => {
  switch (action.type) {
    case "login":
      return {
        ...state,
        user: action.payload.user,
        id: action.payload.id,
        token: action.payload.token,
        refreshToken: action.payload.refreshToken,
      };
    case "logout":
      return {
        ...state,
        user: null,
        id: null,
        token: null,
        refreshToken: null,
        data: null,
      };
    case "refreshtoken":
      return {
        ...state,
        token: action.payload.newAccessToken,
        refreshToken: action.payload.newRefreshToken,
      };
      case "googlesignup":
        return {
            ...state,
            user: action.payload.user,
            id: action.payload.id,
            token: action.payload.token,
            refreshToken: action.payload.refreshToken,
        }
    default:
      return state;
  }
};
const useraddressReducer = (state = {address: null }, action) => {
  switch (action.type) {
    case "addaddress":
      return { ...state, address: action.payload.address };
    case "getalladdress":
      return { ...state, address: action.payload.address };
    default:
      return state;
  }
};
const usertransactionReducer = (state = { transaction: null }, action) => {
    switch (action.type) {
      case "getalltransactions":
        return { ...state, transaction: action.payload.transaction };
      case "addtransactions":
        return { ...state, transaction: action.payload.transaction };
      default:
        return state;
    }
  };

const rootReducer = combineReducers({
  auth: authReducer,
  useraddress: useraddressReducer,
  usertransaction: usertransactionReducer,
});

const persistConfig = {
  key: "root",
  storage,
  whiteList: ["auth", "useraddress", "usertransaction"],
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

export const store = creatstore(persistedReducer, applyMiddleware(thunk));
export const persistor = persistStore(store);
