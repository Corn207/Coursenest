import NavDropdown from "react-bootstrap/NavDropdown";
import { NavLink } from "react-router-dom";
import { useLocation } from "react-router-dom";

import styles from "./Header.module.css";


export default function Header() {
    const pathname = useLocation().pathname;
    return (
        <div className={styles.container}>
            <div className={styles.navLeft}>
                <NavLink to='/' className={`${styles.navLink} ${pathname === '/' ? styles.active : ''}`} >
                    Users
                </NavLink>
                <NavLink to="/courses" className={`${styles.navLink} ${pathname === '/courses' ? styles.active : ''}`}>
                    Courses
                </NavLink>
                <NavLink to="/topics" className={`${styles.navLink} ${pathname === '/topics' ? styles.active : ''}`}>
                    Topics
                </NavLink>
            </div>
            <div className={styles.navRight}>
                <NavDropdown title="Admin Name">
                    <NavDropdown.Item href="#action">
                        My Profile
                    </NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item href="#action">
                        Sign Out
                    </NavDropdown.Item>
                </NavDropdown>
            </div>
        </div>
    );
}
