// import { Table, TableBody, TableCell, TableRow } from '@mui/material';
import classNames from 'classnames/bind';
import PublisherSideBar from '~/components/PublisherPage/PublisherSideBar';

import styles from './Courses.module.scss';
// import DataTable from '@material-ui/core/DataTable';
// import EnhancedTable from './Dashboard';

const cx = classNames.bind(styles);

function Courses() {
    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Courses</p>
            {/* <div className={cx('courses-dashboard')}>
                <div className={cx('mainRow')}>
                    <input type={'checkbox'}></input>
                    <p>Title</p>
                    <p>Description</p>
                    <p>Status</p>
                    <p>Topic</p>
                    <p>Course Tier</p>
                </div>
                <div>Courses</div>
            </div> */}
            {/* <Table><TableBody>
        {data.map((row, index) => (
          <TableRow key={index}>
            <TableCell>{row.column1}</TableCell>
            <TableCell>{row.column2}</TableCell>
            <TableCell>{row.column3}</TableCell>
          </TableRow>
        ))}
      </TableBody></Table> */}
            {/* <DataTable
                title="My Data Table"
                columns={[
                    { title: 'Column 1', field: 'column1' },
                    { title: 'Column 2', field: 'column2' },
                    { title: 'Column 3', field: 'column3' },
                ]}
                data={[
                    { column1: 'Value 1', column2: 'Value 2', column3: 'Value 3' },
                    { column1: 'Value 4', column2: 'Value 5', column3: 'Value 6' },
                    { column1: 'Value 7', column2: 'Value 8', column3: 'Value 9' },
                ]}
            /> */}
        </div>
    );
}

export default Courses;
