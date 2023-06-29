/* eslint-disable */
import React from 'react'
import LoggedUserProvider from '../../store/LoggedUserProvider'

export default function Profile() {
    const isUserAuthenticated = LoggedUserProvider();

    return (
        <div>
            <h1>{isUserAuthenticated}</h1>
             
        </div>
    )
}
