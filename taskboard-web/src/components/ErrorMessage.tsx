type ErrorMessageProps = {
    message: string | null
};

export function ErrorMessage({ message }: ErrorMessageProps) {
    return (
        message === null ? null : <p className="rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-red-700" role="alert">{message}</p>
    );
}