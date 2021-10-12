import React, {ReactNode, useContext, useEffect, useState} from "react";
import {ListResponse} from "../../Api/apiClient";
import {Grid} from "../Grid/Grid";
import "./InfiniteList.scss";
import { LoginContext } from "../../Components/LoginManager/LoginManager";



interface InfiniteListProps<T> {
    fetchItems: (LogOut: () => void, username: string, password: string, page: number, pageSize: number) => Promise<ListResponse<T>>;
    renderItem: (item: T) => ReactNode;
}



export function InfiniteList<T>(props: InfiniteListProps<T>): JSX.Element {
    const [items, setItems] = useState<T[]>([]);
    const [page, setPage] = useState(1);
    const [hasNextPage, setHasNextPage] = useState(false);
    const loginContext = useContext(LoginContext);

    function replaceItems(response: ListResponse<T>) {
        setItems(response.items);
        setPage(response.page);
        setHasNextPage(response.nextPage !== null);
    }

    function appendItems(response: ListResponse<T>) {
        setItems(items.concat(response.items));
        setPage(response.page);
        setHasNextPage(response.nextPage !== null);
    }
    
    useEffect(() => {
        props.fetchItems(loginContext.logOut, loginContext.username, loginContext.password, 1, 10)
            .then(replaceItems);
    }, [props]);

    function incrementPage() {
        props.fetchItems(loginContext.logOut, loginContext.username, loginContext.password, page + 1, 10)
            .then(appendItems);
    }
    
    return (
        <div className="infinite-list">
            <Grid>
                {items.map(props.renderItem)}
            </Grid>
            {hasNextPage && <button className="load-more" onClick={incrementPage}>Load More</button>}
        </div>
    );
}