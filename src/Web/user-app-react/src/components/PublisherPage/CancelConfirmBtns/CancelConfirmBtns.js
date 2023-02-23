import classNames from 'classnames/bind';
import styles from './CancelConfirmBtns.module.scss';

const cx = classNames.bind(styles);

function CancelConfirmBtns() {
    return (
        <div className={cx('confirmBtnsDiv')}>
            <button className={cx('cancelBtn')}>Cancel</button>
            <button className={cx('confirmBtn')}>Confirm</button>
        </div>
    );
}

export default CancelConfirmBtns;
