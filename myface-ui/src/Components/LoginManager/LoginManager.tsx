import React, {createContext, ReactNode, useImperativeHandle, useState} from "react";
import { updateShorthandPropertyAssignment } from "typescript";
import { BrowserRouter as Router, Switch, Route, Redirect } from "react-router-dom";
import { logRoles } from "@testing-library/dom";


export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: () => {},
    logOut: () => {},
    username: "",
    setUsername: (username: string) => {},
    password: "",
    setPassword: (password: string) => {},
    role: 0,
    setRole: (role: number) => {},
    userId: 0,
    setUserId: (userId: number) => {},
    likes: 0,
    setLikes: (likes: number) => {},
    dislikes: 0,
    setDislikes: (likes: number) => {}

});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [role, setRole] = useState(0);
    const [userId, setUserId] = useState(0);
    const [likes, setLikes] = useState(0);
    const [dislikes, setDislikes] = useState(0);

    
    function logIn() {
        
        setLoggedIn(true); 
    }
    
    function logOut() {
        
        setLoggedIn(false);
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
        username: username,
        setUsername: setUsername,
        password: password,
        setPassword: setPassword,
        role: role,
        setRole: setRole,
        userId: userId,
        setUserId: setUserId,
        likes: likes,
        setLikes: setLikes,
        dislikes: dislikes,
        setDislikes: setDislikes,

    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}