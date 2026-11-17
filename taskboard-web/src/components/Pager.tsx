type PagerProps = {
    page: number,
    totalPages: number,
    onChange: (page: number) => void
};

export function Pager({ page, totalPages, onChange }: PagerProps) {
    if (totalPages <= 1) {
        return (
            null
        );
    }
 
    return (
    <div className="flex flex-wrap items-center gap-1.5">
      <button type="button" disabled={page <= 1} onClick={() => onChange(page - 1)}>
        Poprzednia
      </button>
      <span className="text-gray-500">
        Strona {page} z {totalPages}
      </span>
      <button type="button" disabled={page >= totalPages} onClick={() => onChange(page + 1)}>
        Następna
      </button>
    </div>
    );
}