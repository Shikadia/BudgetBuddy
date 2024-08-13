import { createContext, useContext } from 'react';

export class RootStore {
    constructor() {

    }
}

export const Stores = new RootStore();

const StoreContext = createContext(Stores);

export const StoreProvider = ({ children }) => (
    <StoreContext.Provider value={Stores}>{children}</StoreContext.Provider>
  );
  
export const useStore = () => useContext(StoreContext);
  
export default StoreProvider;