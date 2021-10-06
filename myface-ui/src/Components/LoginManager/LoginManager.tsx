import React, {createContext, ReactNode, useImperativeHandle, useState} from "react";
import { updateShorthandPropertyAssignment } from "typescript";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: () => {},
    logOut: () => {},
    username: "",
    password: "",
    // hasUsername: "",
    // hasPassword: "",
    // username: (username : string) => {},
    // password: (password : string) => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    // const [hasUsername, setUsername] = useState("");
    // const [hasPassword, setPassword] = useState("");

    
    function logIn() {
        
        setLoggedIn(true);
        
    }
    
    function logOut() {
        setLoggedIn(false);
    }

    // function username(username : string) {
    //     setUsername(username);
    // }

    // function password(password : string) {
    //     setPassword(password);
    // }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
        // username: hasUsername,
        // password: hasPassword,
        // username: username,
        // password: password,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}