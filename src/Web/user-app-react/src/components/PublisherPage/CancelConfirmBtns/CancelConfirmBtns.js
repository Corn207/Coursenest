import classNames from 'classnames/bind';
import styles from './CancelConfirmBtns.module.scss';

const cx = classNames.bind(styles);

function CancelConfirmBtns({ onCancel }) {
    const handleCancelClick = () => {
        onCancel();
    };
    return (
        <div className={cx('confirmBtnsDiv')}>
            <button className={cx('cancelBtn')} onClick={handleCancelClick}>
                Cancel
            </button>
            <button className={cx('confirmBtn')}>Confirm</button>
        </div>
    );
}

export default CancelConfirmBtns;
