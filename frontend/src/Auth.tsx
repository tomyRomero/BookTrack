import React, { createContext, useReducer, useContext, ReactNode } from 'react';

// Define types for the state
interface AuthState {
  isLoggedIn: boolean;
  token: string | null;
}

// Define action types
type AuthAction = 
  | { type: 'LOGIN'; payload: string } 
  | { type: 'LOGOUT' };

// Initial state
const initialState: AuthState = {
  isLoggedIn: false,
  token: null,
};

// Reducer function with type checking
const authReducer = (state: AuthState, action: AuthAction): AuthState => {
  switch (action.type) {
    case 'LOGIN':
      return {
        ...state,
        isLoggedIn: true,
        token: action.payload,
      };
    case 'LOGOUT':
      return {
        ...state,
        isLoggedIn: false,
        token: null,
      };
    default:
      return state;
  }
};

// Create context with typing
interface AuthContextType {
  state: AuthState;
  dispatch: React.Dispatch<AuthAction>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Context provider component with typing
interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [state, dispatch] = useReducer(authReducer, initialState);

  return (
    <AuthContext.Provider value={{ state, dispatch }}>
      {children}
    </AuthContext.Provider>
  );
};

// Custom hook to use AuthContext with proper typing
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};